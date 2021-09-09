## OSS事件流(OSS.Pipeline)

以 BPM 流程管理为思路，设计的轻量级业务生命周期流程引擎基础框架，
将业务领域对象的流程管控和事件功能抽象剥离，切断事件功能方法内的链式调用，提权至流程引擎统一协调管控，
事件功能作为独立处理单元嵌入业务流程之中，由流程引擎处理事件的触发与消息传递，达成事件处理单元的有效隔离。
由此流程的衔接变成可独立编程的部分，同时向上层提供业务动作的独立扩展，保证业务单元的绝对独立和可复用性，
目的是可以像搭积木一样来完成不同功能代码的集成，系统向真正的低代码平台过渡。

如果将整个业务流当做一个流程管道，结合流程流转的特性，此引擎抽象了三个核心管道组件：

    
### 1. 事件活动组件
    这个组件主要是处理任务的具体内容，如发送短信，执行下单，扣减库存等实际业务操作

### 2. 网关组件
    这个组件主要负责业务流程方向性的逻辑规则处理，如分支，合并流程

### 3. 消息流组件
    这个组件主要负责其他组件之间的消息传递与转化。
       
## 一. 事件活动组件 
这个组件用来实现具体的业务功能逻辑，如关联自动执行活动，中断触发（像用户触发，或消息队列）等活动。
根据 主动/被动 两种情形，同时根据当前活动的业务返回值和下游节点上下文的关系，提供了四类（共七个）基础活动类：

### 1. BaseActivity，BaseActivity<TContext>，BaseActivity<TContext, THandleResult> - 主动触发活动组件
常见如自动审核功能，或者支付成功后自动触发邮件发送等，最简单也是最基本的一种跟随动作处理。
继承此基类，重写Executing方法实现活动内容，同一个流体下实现自动关联执行，执行完毕后自动触发下级节点（传入当前上下文）。

```csharp
        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <returns>
        /// 处理结果
        /// traffic_signal：     
        ///     Green_Pass  - 流体自动流入后续管道
        ///     Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///     Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </returns>
        protected abstract Task<TrafficSignal> Executing();
```
### 2. BaseEffectActivity<THandleResult> ，BaseEffectActivity<TContext, THandleResult>  - 主动触发（受影响上下文）活动组件

默认情况下，当前活动处理结束后当前活动的上下文默认传递给下一个管道节点，实际中可能会出现下游业务活动仅仅需要获取上游的业务活动的执行结果即可，场景如: 下单成功 ==》 发送确认短信，发送短信需要知道订单id。
这种下一个节点受上一个节点结果影响情况，使用此（含Effect）基类，其Executing重写方法的结果将作为下一个活动的上下文信息，如下：

```csharp
        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <returns>
        ///  -（活动是否处理成功，业务结果）
        /// traffic_signal：
        /// traffic_signal：     
        ///     Green_Pass  - 流体自动流入后续管道
        ///     Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///     Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </returns>
        protected abstract Task<TrafficSignal<THandleResult>> Executing();
```


### 3. BasePassiveActivity<TPassivePara, TPassiveResult>  -  被动触发执行活动组件（如需用户参与）

当业务流流入当前组件，业务流动停止，被动等待调用节点的 Execute 方法，外部调用后流程继续向后流动执行（Execute传入的参数作为后续的上下文）。
继承此基类（含Passive），重写Executing方法实现具体业务逻辑内容。

```csharp
        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="para">当前活动上下文信息</param>
        /// <returns>
        ///  -（活动是否处理成功，业务结果）
        /// traffic_signal：
        /// traffic_signal：     
        ///     Green_Pass  - 流体自动流入后续管道
        ///     Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///     Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </returns>
        protected abstract Task<TrafficSignal<THandleResult>> Executing(THandlePara para);        
```

### 4. BasePassiveEffectActivity<TPassivePara, TPassiveResult>  -  被动触发（受影响上下文）执行活动组件

同主动触发活动组件一样，当前活动处理业务结果作为下游节点的上下文。
继承此基类（含Passive和Effect），重写Executing方法实现具体业务逻辑内容。

```csharp
        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="para">当前活动上下文信息</param>
        /// <returns>
        ///  -（活动是否处理成功，业务结果）
        /// traffic_signal：
        /// traffic_signal：     
        ///     Green_Pass  - 流体自动流入后续管道
        ///     Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///     Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </returns>
        protected abstract Task<TrafficSignal<THandleResult>> Executing(THandlePara para);      
```
以上四类基类都包含  Execute 方法，可供流体从任意节点直接启动向下执行。


## 二. 网关组件
此组件主要负责逻辑的规则处理，业务的走向逻辑无非分与合，这里给出两个基类：

### 1. BaseAggregateGateway<TContext> - 聚合业务分支流程活动组件
将多条业务分支聚合到当前网关组件下，由当前网关统一控制是否将业务流程向后传递，只需要继承此基类重写IfMatchCondition 方法即可

### 2. BaseBranchGateway<TContext> - 分支网关组件
此组件将业务分流处理，定义流体时通过AddBranchPipe添加多个分支，至于如何分流，只需要继承此基类重写FilterNextPipes方法即可，你也可以在此之上实现BPMN中的几种网关类型（并行，排他，和包含）。

## 三. 消息流组件
此组件主要负责消息的传递和转化处理，根据是否需要转化，或者异步定义四个基类如下：

### 1. BaseMsgConverter<TInMsg, TOutMsg> - 转化连接组件
业务流经过此组件，直接执行Convert方法（自定义实现），转化成对应的下个组件执行参数，自动进入下个组件。

### 2. BaseMsgFlow<TMsg> - 异步缓冲数据连接组件（提供默认实现：MsgFlow<TMsg>）
此前组件的流动以【发布/订阅】的方式异步执行，触发来源可以方便的修改为队列或数据库，详情【OSS.DataFlow】[https://github.com/KevinWG/OSS.DataFlow]）

### 3. BaseMsgPublisher<TMsg> 消息发布者组件 - （提供默认实现：MsgPublisher<TMsg>）
此前组件提供数据的【发布】方式，触发来源可以方便的修改为队列或数据库，详情【OSS.DataFlow】[https://github.com/KevinWG/OSS.DataFlow]）
 
### 4. BaseMsgSubscriber<TMsg> - 消息订阅者组件（提供默认实现：MsgSubscriber<TMsg>）
此前组件提供数据的【订阅】方式，触发来源可以方便的修改为队列或数据库，详情【OSS.DataFlow】[https://github.com/KevinWG/OSS.DataFlow]）


以上是三个核心的组件部分，以上三个组件任意组合可以组成PipeLine（流体），PipeLine本身又可以作为一个组件加入到一个更大的流体之中，通过流体的 ToRoute() 方法，可以获取对应的内部组件关联路由信息。
        
## 四. 简单示例场景

首先我们假设当前有一个进货管理的场景，需经历  进货申请，申请审批，购买支付，入库（同时邮件通知申请人） 几个环节，每个环节表示一个事件活动，比如申请活动我们定义如下：
```csharp
    public class ApplyActivity : BaseEffectActivity<ApplyContext, long>
    {
        public ApplyActivity()
        {
            PipeCode = "ApplyActivity";
        }
        
        protected override Task<TrafficSignal<long>> Executing(ApplyContext para)
        {
            LogHelper.Info($"发起 [{para.name}] 采购申请");
            return Task.FromResult(new TrafficSignal<long>(100000001L));
        }
    }
```

我们设定申请后审核自动执行，审核成功等待支付（被动类型）。 相同的处理方式我们定义剩下几个环节事件，列表如下：
```csharp
    ApplyActivity      - 申请事件    (参数：ApplyContext)
    AutoAuditActivity  - 审核事件    (参数：long)
    PayActivity        - 购买事件    (参数：PayContext)
    StockActivity      - 入库事件    (参数：StockContext)
    EmailActivity      - 发送邮件事件    (参数：SendEmailContext)
```
以上五个事件活动，其具体实现和参数完全独立，同时因为购买支付后邮件和入库是相互独立的事件，定义分支网关做分流（规则）处理，代码如下：
```csharp
    public class PayGateway : BaseBranchGateway<PayContext>
    {
        public PayGateway()
        {
            PipeCode = "PayGateway";
        }

        protected override bool FilterBranchCondition(PayContext branchContext, IPipe branch, string prePipeCode)
        {
            LogHelper.Info($"通过{PipeCode}  判断分支 {branch.PipeCode} 是否满足分流条件！");
            return base.FilterBranchCondition(branchContext, branch, prePipeCode);
        }
    }
```
这里的意思相对简单，即传入的所有的分支不用过滤，直接全部分发。

同样因为五个事件的方法参数不尽相同，中间的我们添加消息连接器，作为消息的中转和转化处理（也可以在创建流体时表达式处理），以支付参数到邮件的参数转化示例：
```csharp
    public class PayEmailConnector : BaseMsgConverter<PayContext, SendEmailContext>
    {
        public PayEmailConnector()
        {
            PipeCode = "PayEmailConnector";
        }
        protected override SendEmailContext Convert(PayContext inContextData)
        {
            // ......
            return new SendEmailContext() { body = $" 您成功支付了订单，总额：{inContextData.money}" };
        }
    }
```

通过以上，申购流程的组件定义完毕，串联使用如下（这里是单元测试类，实际业务我们可以创建一个Service处理）：

```csharp
    [TestClass]
    public class BuyFlowTests
    {
        public readonly ApplyActivity     ApplyActivity = new ApplyActivity();
        public readonly AutoAuditActivity AuditActivity = new AutoAuditActivity();

        public readonly PayActivity PayActivity = new PayActivity();

        public readonly PayGateway PayGateway = new PayGateway();

        public readonly StockConnector StockConnector = new StockConnector();
        public readonly StockActivity  StockActivity  = new StockActivity();

        public readonly PayEmailConnector EmailConnector = new PayEmailConnector();
        public readonly SendEmailActivity EmailActivity  = new SendEmailActivity();



        private EndGateway _endNode = new EndGateway();

        //  构造函数内定义流体关联
        public BuyFlowTests()
        {


            ApplyActivity
            .Append(AuditActivity)

            .Append(PayActivity)
            .Append(PayGateway);

            // 网关分支 - 发送邮件分支
            PayGateway
            .Append(EmailConnector)
            .Append(EmailActivity)
            .Append(_endNode);

            // 网关分支- 入库分支
            PayGateway
            .Append(StockConnector)
            .Append(StockActivity)
            .Append(_endNode);


        }

        [TestMethod]
        public async Task FlowTest()
        {
            await ApplyActivity.Execute(new ApplyContext()
            {
                name = "冰箱"
            });

            // 延后一秒，假装有支付操作
            await Task.Delay(1000);

            await PayActivity.Execute(new PayContext()
            {
                count = 10,
                money = 10000
            });
            await Task.Delay(1000);// 等待异步日志执行完成
        }

        [TestMethod]
        public void RouteTest()
        {
            var TestPipeline = new Pipeline<ApplyContext, Empty>("test-flow", ApplyActivity, _endNode);

            var route = TestPipeline.ToRoute();
            Assert.IsTrue(route != null);
        }
    }
```
运行单元测试，结果如下：

```
xxxxxx 17:46:15   Detail: 通过ApplyActivity发起 [冰箱] 采购申请

xxxxxx 17:46:15   Detail:通过AuditActivity 自动审核通过申请（编号：100000001）

xxxxxx 17:46:16   Detail:通过PayActivity 支付动作执行,数量：10，金额：10000）

xxxxxx 17:46:16   Detail:通过PayGateway  判断分支 PayEmailConnector 是否满足分流条件！

xxxxxx 17:46:16   Detail:通过PayGateway  判断分支 StockConnector 是否满足分流条件！

xxxxxx 17:46:16   Detail:分流-1（SendEmailActivity）邮件发送，内容： 您成功支付了订单，总额：10000

xxxxxx 17:46:17         Detail: 通过 SendEmailActivity 管道进入结束网关！

xxxxxx 17:46:17   Detail:分流-2（StockActivity）增加库存，数量：10

xxxxxx 17:46:17         Detail: 通过 StockActivity 管道进入结束网关！

```