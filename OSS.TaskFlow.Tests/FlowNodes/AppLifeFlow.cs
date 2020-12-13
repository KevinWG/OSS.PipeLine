using OSS.EventFlow;
using OSS.EventFlow.Connector;
using OSS.EventFlow.Interface;
using OSS.TaskFlow.Tests.FlowNodes.Apply;
using OSS.TaskFlow.Tests.FlowNodes.Audit;
using OSS.TaskFlow.Tests.FlowNodes.Pay;
using OSS.TaskFlow.Tests.FlowNodes.Stock;

namespace OSS.TaskFlow.Tests.FlowNodes
{
    /// <summary>
    ///  假设的一个进货生命周期管理
    ///   1. 发起进货申请
    ///     2. 进货申请审核
    ///     3. 进货购买支付
    ///     4. 入库
    /// </summary>
    public class AppLifeFlow : BaseFlow<ApplyContext, StockContext>
    {
        public static readonly ApplyActivity ApplyActivity = new ApplyActivity();
        public  readonly AutoAuditActivity AutoAuditActivity = new AutoAuditActivity();

        public readonly PayConnector PayConnector = new PayConnector();
        public readonly PayActivity PayActivity = new PayActivity();

        public readonly StockConnector StockConnector = new StockConnector();
        public readonly StockActivity StockActivity = new StockActivity();

        public AppLifeFlow()
        {
            ApplyActivity.Append(AutoAuditActivity).Append(PayConnector).Append(PayActivity).Append(StockConnector).Append(StockActivity);
        }

        protected override BasePipe<ApplyContext> InitialFirstPipe() => ApplyActivity;

        protected override IPipeAppender<StockContext> InitialLastPipe() => StockActivity;
    }


    public class PayConnector:BaseConnector<ApplyContext, PayContext>
    {
        protected override PayContext Convert(ApplyContext inContextData)
        {
           return new PayContext(){id = inContextData.id};
        }
    }

    public class StockConnector : BaseConnector<PayContext, StockContext>
    {
        protected override StockContext Convert(PayContext inContextData)
        {
            return new StockContext() { id = inContextData.id };
        }
    }
}
