using OSS.EventFlow.Connector;
using OSS.TaskFlow.Tests.FlowNodes.Apply;

namespace OSS.TaskFlow.Tests.FlowNodes.Pay
{
    public class PayConnector:BaseConnector<ApplyContext, PayContext>
    {
        protected override PayContext Convert(ApplyContext inContextData)
        {
            return new PayContext(){id = inContextData.id};
        }
    }
}