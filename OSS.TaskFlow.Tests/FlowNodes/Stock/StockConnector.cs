using OSS.EventFlow.Connector;
using OSS.TaskFlow.Tests.FlowNodes.Pay;

namespace OSS.TaskFlow.Tests.FlowNodes.Stock
{
    public class StockConnector : BaseConnector<PayContext, StockContext>
    {
        protected override StockContext Convert(PayContext inContextData)
        {
            return new StockContext() { id = inContextData.id };
        }
    }
}