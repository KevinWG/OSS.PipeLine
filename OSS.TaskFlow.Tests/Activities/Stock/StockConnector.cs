using OSS.EventFlow.Connector;
using OSS.TaskFlow.Tests.Activities.Pay;

namespace OSS.TaskFlow.Tests.Activities.Stock
{
    public class StockConnector : BaseConnector<PayContext, StockContext>
    {
        protected override StockContext Convert(PayContext inContextData)
        {
            return new StockContext() { id = inContextData.id };
        }
    }
}