# OSS事件流(OSS.EventFlow)

以BPMN 2.0 流程管理为思路，设计的轻量级业务生命周期流程引擎基础框架，将业务过程的实际业务处理动作和衔接动作的流程抽象分离，
将穿插在动作内部的不易见的流程部分剥离，变成可独立编程的部分，同时向上层提供业务动作的独立扩展，保证业务单元的绝对独立和可复用性，
可以像搭积木一样来完成不同功能代码的集成，搭建真正的低代码平台。

如果将整个业务流当做一个流程管道，结合流程流转的特性，此引擎抽象了三个核心管道组件：
    a. 事件活动组件
        这个组件主要是处理任务的具体内容，如发送短信，执行下单，扣减库存等实际业务操作
    b. 网关组件
        这个组件主要负责业务流程方向性的逻辑规则处理，如分支，合并流程
    c. 连接器组件
        这个组件主要负责其他组件之间的消息传递与转化。
        

## 一. 事件活动组件
   这个组件就是业务的动作本身，根据任务触发的特性，如关联自动执行，中断触发（如用户触发，或消息队列等），根据这两种情形，提供了两个抽象基类：

   1. BaseActivity<TContext> - 直接执行活动组件
    常见如自动审核功能，或者支付成功后自动触发邮件发送等，最简单也是最基本的一种动作处理。 继承此基类，重写Executing方法实现活动内容，同一个流体下实现自动关联执行。
    如果Executing方法返回False，则触发Block，业务流不再向后续管道传递
    返回True，则流体自动流入后续管道

   2. BaseActionActivity<TContext, TResult> - 用户触发活动组件
    继承此基类 ，重写Executing方法（自定义返回结果类型）实现活动内容。 当业务流流入当前组件时，触发调用Notice（虚方法可重写），之后业务流动停止，
    当用户触发时，显式调用 Action 方法（内部调用Executing返回自定义结果类型），流程继续向后流动执行。

## 二. 网关组件
    此组件主要负责逻辑的规则处理，业务的走向逻辑无非分与合，这里给出两个基类：
    1. BaseAggregateGateway<TContext> - 聚合业务分支流程活动组件
    将多条业务分支聚合到当前网关组件下，由当前网关统一控制是否将业务流程向后传递，只需要继承此基类重写IfMatchCondition 方法即可

    2. BaseBranchGateway<TContext> - 分支网关组件
    此组件将业务分流处理，定义流体时通过AddBranchPipe添加多个分支，至于如何分流，只需要继承此基类重写FilterNextPipes方法即可，你也可以在此之上实现BPMN中的几种网关类型（并行，排他，和包含）。

## 三. 连接器
    此组件主要负责消息的传递和转化处理，在消息的传递过程中又支持直接传递和异步缓冲（IBufferTunnel）传递，根据是否需要转化，或者异步定义三个基类如下：

    1. BaseConnector<InContext, OutContext> - 转化连接组件
    业务流经过此组件，直接执行Convert方法（需重写），转化成对应的下个组件执行参数，自动进入下个组件。

    2. BaseBufferConnector<TContext> - 异步缓冲连接组件，继承IBufferTunnel接口
    继承此组件后，必须重写Push方法，实现异步缓冲保存的处理，业务流进入此组件后，调用Push方法保存，之后业务流动停止，
    消息唤醒时，需显式调用Pop方法，业务流继续向后执行

    3. BaseBufferConnector<InContext, OutContext> - 异步缓冲+转化 连接组件
    继承此组件后，必须重写Push，Convert方法，实现异步缓冲保存和转化的处理，业务流进入此组件后，同样调用Push方法保存，之后业务流动停止，
    消息唤醒时，需显式调用Pop方法（内部调用Convert方法，完成参数转化），业务流继续向后执行
        
## 四. 简单示例场景
首先我们假设当前有一个进货管理的场景，需经历  进货申请，申请审批，购买支付，入库（同时邮件通知申请人） 几个环节，每个环节表示一个事件活动，比如申请活动我们定义如下：

``` csharper
    public class ApplyActivity : BaseActivity<ApplyContext>
    {
        protected override Task<bool> Executing(ApplyContext data)
        {
            // ......
            LogHelper.Info("这里刚才发生了一个采购申请"); 
            return Task.FromResult(true);
        }
    }
```
这里为了方便观察，直接继承 BaseActivity，即多个活动连接后，自动运行。 相同的处理方式我们定义剩下几个环节事件，列表如下：
``` csharper
    ApplyActivity      - 申请事件    (参数：ApplyContext)
    AutoAuditActivity  - 审核事件    (参数：ApplyContext)
    PayActivity        - 购买事件    (参数：PayContext)
    StockActivity      - 入库事件    (参数：StockContext)
    EmailActivity      - 发送邮件事件    (参数：SendEmailContext)
```
以上五个事件活动，其具体实现和参数完全独立，同时因为购买支付后邮件和入库是相互独立的事件，定义分支网关做分流（规则）处理，代码如下：

``` csharper
    public class PayGateway:BaseBranchGateway<PayContext>
    {
        protected override IEnumerable<BasePipe<PayContext>> FilterNextPipes(List<BasePipe<PayContext>> branchItems, PayContext context)
        {
            // ......
            LogHelper.Info("这里进行支付通过后的分流");
            return branchItems;
        }
    }
```
这里的意思相对简单，即传入的所有的分支不用过滤，直接全部分发。

同样因为五个事件的方法参数不尽相同，中间的我们添加消息连接器，作为消息的中转和转化处理（也可以在创建流体时表达式处理），以支付参数到邮件的参数转化示例：
``` csharper
    public class PayEmailConnector : BaseConnector<PayContext, SendEmailContext>
    {
        protected override SendEmailContext Convert(PayContext inContextData)
        {
            // ......
            return new SendEmailContext() { id = inContextData.id };
        }
    }
```

通过以上，申购流程的组件定义完毕，串联使用如下（这里是单元测试类，实际业务我们可以创建一个Service处理）：

``` csharper
        public readonly ApplyActivity ApplyActivity = new ApplyActivity();
        public readonly AuditActivity AuditActivity = new AuditActivity();

        public readonly PayActivity PayActivity = new PayActivity();

        public readonly PayGateway PayGateway = new PayGateway();

        public readonly StockConnector StockConnector = new StockConnector();
        public readonly StockActivity  StockActivity  = new StockActivity();

        public readonly PayEmailConnector EmailConnector = new PayEmailConnector();
        public readonly SendEmailActivity EmailActivity  = new SendEmailActivity();

          //  构造函数内定义流体关联
        public BuyFlowTests()
        {
            ApplyActivity
            .Append(AuditActivity)
            .AppendConvert(applyContext => new PayContext() {id = applyContext.id})// 表达式方式的转化器
            .Append(PayActivity)
            .Append(PayGateway);

            // 网关分支 - 发送邮件分支
            PayGateway.AddBranchPipe(EmailConnector)
            .Append(EmailActivity);

            // 网关分支- 入库分支
            PayGateway.AddBranchPipe(StockConnector)
            .Append(StockActivity);
            //.Append(后续事件)
        }


        [TestMethod]
        public async Task FlowTest()
        {
            await ApplyActivity.Start(new ApplyContext()
            {
                id = "test_business_id"
            });
        }
```
运行单元测试，结果如下：

```
xxx Detail:这里刚才发生了一个采购申请

xxx Detail:管理员审核通过

xxx Detail:发起支付处理

xxx Detail:这里进行支付通过后的分流

xxx Detail:分流-1.邮件发送

xxx Detail:分流-2.库存保存

```