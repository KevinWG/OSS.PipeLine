using OSS.EventFlow;
using OSS.EventFlow.Interface;
using OSS.TaskFlow.Tests.Activities.Apply;
using OSS.TaskFlow.Tests.Activities.Audit;
using OSS.TaskFlow.Tests.Activities.Pay;
using OSS.TaskFlow.Tests.Activities.Stock;

namespace OSS.TaskFlow.Tests
{
    /// <summary>
    ///  假设的一个进货生命周期管理
    ///     1. 发起进货申请
    ///     2. 进货申请审核
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
}
