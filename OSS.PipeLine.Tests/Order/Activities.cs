using System.Collections.Generic;
using OSS.Tools.Log;
using System.Threading.Tasks;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.Tests.Order
{
    public class OrderPayReq
    {
        public long OrderId { get; set; }
        public decimal PayMoney { get; set; }
    }

    /// <summary>
    ///  订单支付管道
    ///    OrderPayReq - 业务输入参数，  bool - 业务输出执行成功失败，   long - 逻辑输出订单Id
    /// </summary>
    internal class OrderPay : BaseActivity<OrderPayReq, bool, long>
    {
        protected override async Task<TrafficSignal<bool, long>> Executing(OrderPayReq para)
        {
            LogHelper.Info($"支付订单（{para.OrderId}）金额：{para.PayMoney} 成功");

            await Task.Delay(10);

            // 返回执行成功，并告诉下级管道 订单Id
            return new TrafficSignal<bool, long>(true, para.OrderId);
        }
    }

    /// <summary>
    ///  支付Hook
    ///     long-是上级管道传入的订单Id， bool - 业务输出执行成功失败，  List<NotifyMsg> 需要发送的消息列表 
    /// </summary>
    internal class PayHook : BaseActivity<long, bool, List<NotifyMsg>>
    {
        protected override async Task<TrafficSignal<bool, List<NotifyMsg>>> Executing(long para)
        {
            LogHelper.Info($"执行订单（{para}）Hook");
            await Task.Delay(10);

            var msgs = new List<NotifyMsg>
            {
                new NotifyMsg() {target = "管理员", content = $"订单（{para}）支付成功，请注意发货"},
                new NotifyMsg() {target = "用户", content  = $"订单（{para}）支付成功，已经入服务流程", is_sms = true}
            };

            return new TrafficSignal<bool, List<NotifyMsg>>(true, msgs);
        }
    }


    public class NotifyMsg
    {
        public string target { get; set; }
        public string content { get; set; }
        public bool is_sms { get; set; } // 假设不是短信就是邮件
    }

    /// <summary>
    ///  发送短信服务
    ///     NotifyMsg - 上级管道传递的业务输入参数，   bool - 当前业务执行成功失败
    /// </summary>
    internal class NotifySMS : BaseActivity<NotifyMsg, bool>
    {
        protected override async Task<TrafficSignal<bool>> Executing(NotifyMsg para)
        {
            LogHelper.Info($"发送用户短信消息 ：{para.target}:{para.content}");

            await Task.Delay(10);

            return new TrafficSignal<bool>(true);
        }
    }

    /// <summary>
    ///  发送邮件服务
    ///     NotifyMsg - 上级管道传递的业务输入参数，   bool - 当前业务执行成功失败
    /// </summary>
    internal class NotifyEmail : BaseActivity<NotifyMsg, bool>
    {
        protected override async Task<TrafficSignal<bool>> Executing(NotifyMsg para)
        {
            LogHelper.Info($"发送管理员邮件消息 ：{para.target}:{para.content}");

            await Task.Delay(10);

            return new TrafficSignal<bool>(true);
        }
    }

    internal class OrderPayPipeline
    {
        private static readonly OrderPay    _pay       = new OrderPay();
        private static readonly PayHook     _payHook   = new PayHook();

        private static readonly SimpleBranchGateway<NotifyMsg> _notifyGateway   = new SimpleBranchGateway<NotifyMsg>();

        private static readonly NotifySMS             _notifySms   = new NotifySMS();
        private static readonly NotifyEmail           _notifyEmail = new NotifyEmail();

        private static readonly EmptyActivity _end = new EmptyActivity();

        static OrderPayPipeline()
        {
            _pay
                .AppendMsgFlow("order_pay_event") // 添加默认实现的异步消息队列中
                .Append(_payHook)                 // 消息队列数据流向hook管道
                .AppendMsgEnumerator()            // Hook处理后有多条消息，添加消息枚举器
                .Append(_notifyGateway);          //  枚举后的单个消息体流入发送分支网关

            _notifyGateway.Append(m => m.is_sms, _notifySms).Append(_end);
            _notifyGateway.Append(m => !m.is_sms, _notifyEmail).Append(_end);


            // 添加日志，通过初始化流水线，给流水线添加Watcher，会自动给下边的所有Pipe添加Watcher
            _pay.AsPipeline(_end, new PipeLineOption() { Watcher = new FlowWatcher() },"OrderPayPipeline");
        }

        // 作为对外暴露接口
        public Task<bool> PayOrder(OrderPayReq req)
        {
            return _pay.Execute(req);
        }
    }

    public class FlowWatcher : IPipeLineWatcher
    {
        public Task PreCall(string pipeCode, PipeType pipeType, object input)
        {
            LogHelper.Info($"进入 {pipeCode} 管道", "PipePreCall", "PipelineWatcher");
            return Task.CompletedTask;
        }

        public Task Executed(string pipeCode, PipeType pipeType, object input, WatchResult watchResult)
        {
            LogHelper.Info($"管道 {pipeCode} 执行结束，结束信号：{watchResult.signal}", "PipeExecuted", "PipelineWatcher");
            return Task.CompletedTask;
        }

        public Task Blocked(string pipeCode, PipeType pipeType, object input, WatchResult watchResult)
        {
            LogHelper.Info($"管道 {pipeCode} 阻塞", "PipeBlocked", "PipelineWatcher");
            return Task.CompletedTask;
        }
    }
}
