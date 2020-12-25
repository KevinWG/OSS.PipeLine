# OSS事件流(OSS.EventFlow)

以BPMN 2.0 流程管理为思路，设计的轻量级流程引擎基础框架，通过继承抽象基类，可以像搭积木一样来完成不同功能代码的集成。

根据流程流转的特性，此流抽象了三个核心对象：
    a. 事件活动对象
        这个对象主要是处理任务的具体内容，如发送短信，执行下单，扣减库存等操作
    b. 网关
        

一. 活动或者任务
    程序中常见的任务方式，有关联自动执行，用户主动触发，定时或脚本(如定时器，消息队列)触发，根据这三种情形，提供了三个抽象基类：
   1. BaseActivity-自动执行
        继承此基类，重写Executing方法实现活动内容，实现自动关联执行
    2. BaseActionActivity- 用户触发
      继承此基类 ，重写Executing方法（自定义返回结果类型）实现活动内容，流体进入当前活动时触发调用Notice（虚方法可重写），之后停止，
      用户触发时调用 Action 方法（返回自定义结果类型）即可继续执行。
    3. BaseSuspendActivity-定时等异步缓冲触发
        继承此基类，重写Executing方法实现活动内容，重写Suspend方法在流体进入时触发（如将消息写入消息队列），
        异步唤起时（如消费消息队列消息）调用Resume方法即可继续执行。
        

简单示例：

    /// <summary>
    ///  假设的一个进货生命周期管理
    ///     1. 发起进货申请
    ///     2. 进货申请审核（自动执行）
    ///     3. 进货购买支付
    ///     4. 入库
    /// </summary>
    public class AppLifeFlow : BaseFlow<ApplyContext, StockContext>
    {
        public static readonly ApplyActivity ApplyActivity = new ApplyActivity();
        public readonly AutoAuditActivity AutoAuditActivity = new AutoAuditActivity();

        public readonly PayConnector PayConnector = new PayConnector();
        public readonly PayActivity PayActivity = new PayActivity();

        public readonly StockConnector StockConnector = new StockConnector();
        public readonly StockActivity StockActivity = new StockActivity();

        public AppLifeFlow()
        {
            ApplyActivity
                .Append(AutoAuditActivity)
                .Append(PayConnector)
                .Append(PayActivity)
                .Append(StockConnector)
                .Append(StockActivity);
        }

        protected override BasePipe<ApplyContext> InitialFirstPipe() => ApplyActivity;

        protected override IPipeAppender<StockContext> InitialLastPipe() => StockActivity;
    }