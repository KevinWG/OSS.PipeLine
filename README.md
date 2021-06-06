## OSS事件流(OSS.Pipeline)

以BPMN 2.0 流程管理为思路，设计的轻量级业务生命周期流程引擎基础框架，
将业务领域对象的流程管控和事件功能抽象剥离，切断事件功能方法内的链式调用，提权至流程引擎统一协调管控，
事件功能作为独立处理单元嵌入业务流程之中，由流程引擎处理事件的触发与消息传递，达成事件处理单元的有效隔离。
由此流程的衔接变成可独立编程的部分，同时向上层提供业务动作的独立扩展，保证业务单元的绝对独立和可复用性，
目的是可以像搭积木一样来完成不同功能代码的集成，系统向真正的低代码平台过渡。


如果将整个业务流当做一个流程管道，结合流程流转的特性，此引擎抽象了三个核心管道组件：

    
#### 1. 事件活动组件
    这个组件主要是处理任务的具体内容，如发送短信，执行下单，扣减库存等实际业务操作

#### 2. 网关组件
    这个组件主要负责业务流程方向性的逻辑规则处理，如分支，合并流程

#### 3. 连接器组件
    这个组件主要负责其他组件之间的消息传递与转化。
       
## 一. 事件活动组件 
这个组件用来实现具体的业务功能逻辑，根据业务功能触发的特性，如关联自动执行，中断触发（像用户触发，或消息队列等），根据 主动/被动 两种情形，提供了两个抽象基类：

####1. BaseActivity<TContext> - 主动触发执行活动组件
    常见如自动审核功能，或者支付成功后自动触发邮件发送等，最简单也是最基本的一种跟随动作处理。 继承此基类，重写Executing方法实现活动内容，同一个流体下实现自动关联执行。
```csharp
        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="contextData">当前活动上下文信息</param>
        /// <returns>
        /// 处理结果
        /// False - 触发Block，业务流不再向后续管道传递。
        /// True  - 流体自动流入后续管道
        /// </returns>
        protected abstract Task<bool> Executing(TContext contextData);
```

默认情况下，当前活动处理结束后当前活动的上下文默认传递给下一个管道节点，实际中可能会出现下游业务活动仅仅需要获取上游的业务活动的执行结果即可，场景如: 下单成功 ==》 发送确认短信，发送短信需要知道订单id。
这种下一个节点受上一个节点结果影响情况，框架提供了一个 BaseEffectActivity<TContext, TResult> 变体基类，其Executing方法的结果将作为下一个活动的上下文信息，如下：

```csharp
        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="contextData">当前活动上下文信息</param>
        /// <returns>
        /// (bool is_ok,TResult result)-（活动是否处理成功，业务结果）
        /// is_ok：
        ///     False - 触发Block，业务流不再向后续管道传递。
        ///     True  - 流体自动流入后续管道
        /// </returns>
        protected abstract Task<(bool is_ok, TResult result)> Executing(TContext contextData);
```


####2. BaseFuncActivity<TInContext, TFuncPara, TFuncResult> -  被动触发执行活动组件（如用户主动点击，或定时任务触发）

当业务流流入当前组件时，触发调用 Notice(context-上下文)，之后业务流动停止，被动等待调用节点的 Execute 方法，外部调用后流程继续向后流动执行（当前执行结果作为下一个节点的上下文传递）。
继承此基类 ，重写Executing方法（自定义返回结果类型）实现具体业务逻辑内容。

```csharp
        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="contextData">当前活动上下文信息</param>
        /// <returns>
        /// (bool is_ok,TResult result)-（活动是否处理成功，业务结果）
        /// is_ok：
        ///     False - 触发Block，业务流不再向后续管道传递。
        ///     True  - 流体自动流入后续管道
        /// </returns>
        protected abstract Task<(bool is_ok, TFuncResult result)> Executing(TFuncPara contextData);

        /// <summary>
        ///  消息进入通知
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task<bool> Notice(TInContext data)
        {
            return InterUtil.TrueTask;
        }        
```


## 二. 网关组件
此组件主要负责逻辑的规则处理，业务的走向逻辑无非分与合，这里给出两个基类：

####1. BaseAggregateGateway<TContext> - 聚合业务分支流程活动组件
    将多条业务分支聚合到当前网关组件下，由当前网关统一控制是否将业务流程向后传递，只需要继承此基类重写IfMatchCondition 方法即可

####2. BaseBranchGateway<TContext> - 分支网关组件
    此组件将业务分流处理，定义流体时通过AddBranchPipe添加多个分支，至于如何分流，只需要继承此基类重写FilterNextPipes方法即可，你也可以在此之上实现BPMN中的几种网关类型（并行，排他，和包含）。

## 三. 连接器组件
此组件主要负责消息的传递和转化处理，根据是否需要转化，或者异步定义三个基类如下：

####1. BaseConnector<InContext, OutContext> - 转化连接组件
    业务流经过此组件，直接执行Convert方法（自定义实现），转化成对应的下个组件执行参数，自动进入下个组件。

####2. BaseBufferConnector<InContext, OutContext> - 异步缓冲转化连接组件
    （此前组件的流动以【发布/订阅】的方式异步执行，触发来源可以方便的修改为队列或数据库，详情【OSS.DataFlow】[https://github.com/KevinWG/OSS.DataFlow]）


以上是三个核心的组件部分，以上三个组件任意组合可以组成PipeLine（流体），PipeLine本身又可以作为一个组件加入到一个更大的流体之中，通过流体的 ToRoute() 方法，可以获取对应的内部组件关联路由信息。
        
## 四. 简单示例场景
首先我们假设当前有一个进货管理的场景，需经历  进货申请，申请审批，购买支付，入库（同时邮件通知申请人） 几个环节，每个环节表示一个事件活动，比如申请活动我们定义如下：
```csharp
    public class ApplyActivity : BaseActivity<ApplyContext>
    {
        protected override Task<bool> Executing(ApplyContext data)
        {
            LogHelper.Info("这里刚才发生了一个采购申请");
            return InterUtil.TrueTask;
        }
    }
```
这里为了方便观察，直接继承 BaseActivity，即多个活动连接后，自动运行。 相同的处理方式我们定义剩下几个环节事件，列表如下：
```csharp
    ApplyActivity      - 申请事件    (参数：ApplyContext)
    AutoAuditActivity  - 审核事件    (参数：ApplyContext)
    PayActivity        - 购买事件    (参数：PayContext)
    StockActivity      - 入库事件    (参数：StockContext)
    EmailActivity      - 发送邮件事件    (参数：SendEmailContext)
```
以上五个事件活动，其具体实现和参数完全独立，同时因为购买支付后邮件和入库是相互独立的事件，定义分支网关做分流（规则）处理，代码如下：
```csharp
    public class PayGateway : BaseBranchGateway<PayContext>
    {
        protected override IEnumerable<BasePipe<PayContext>> FilterNextPipes(List<BasePipe<PayContext>> branchItems, PayContext context)
        {
            LogHelper.Info("这里进行支付通过后的分流");
            return branchItems;
        }
    }
```
这里的意思相对简单，即传入的所有的分支不用过滤，直接全部分发。

同样因为五个事件的方法参数不尽相同，中间的我们添加消息连接器，作为消息的中转和转化处理（也可以在创建流体时表达式处理），以支付参数到邮件的参数转化示例：
```csharp
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

```csharp
    [TestClass]
    public class BuyFlowTests
    {
        public readonly ApplyActivity ApplyActivity = new ApplyActivity();
        public readonly AuditActivity AuditActivity = new AuditActivity();

        public readonly PayActivity PayActivity = new PayActivity();

        public readonly PayGateway PayGateway = new PayGateway();

        public readonly StockConnector StockConnector = new StockConnector();
        public readonly StockActivity StockActivity = new StockActivity();

        public readonly PayEmailConnector EmailConnector = new PayEmailConnector();
        public readonly SendEmailActivity EmailActivity = new SendEmailActivity();


        public readonly PipeLine<ApplyContext, bool> PipeLine;
        //  构造函数内定义流体关联
        public BuyFlowTests()
        {
            var endActivity = new EmptyActivity<bool>();

            ApplyActivity
            .Append(AuditActivity)
            .AppendConvert(applyContext => new PayContext() { id = applyContext.id })// 表达式方式的转化器
            .Append(PayActivity)
            .Append(PayGateway);

            // 网关分支 - 发送邮件分支
            PayGateway.AddBranchPipe(EmailConnector)
            .Append(EmailActivity).Append(endActivity);

            // 网关分支- 入库分支
            PayGateway.AddBranchPipe(StockConnector)
            .Append(StockActivity).Append(endActivity); 
            //.Append(后续事件)

            // 流体对象
            PipeLine = ApplyActivity.AsFlowStartAndEndWith(endActivity);
        }


        [TestMethod]
        public async Task FlowTest()
        {
            await PipeLine.Start(new ApplyContext()
            {
                id = "test_business_id"
            });
            await Task.Delay(2000);
        }

        [TestMethod]
        public void RouteTest()
        {
            var route = PipeLine.ToRoute();

            Assert.IsTrue(route != null);
        }
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