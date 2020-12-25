using OSS.EventFlow.Connector;
using OSS.TaskFlow.Tests.Activities.Apply;

namespace OSS.TaskFlow.Tests.Activities.Pay
{
    public class PayConnector:BaseConnector<ApplyContext, PayContext>
    {
        protected override PayContext Convert(ApplyContext inContextData)
        {
            return new PayContext(){id = inContextData.id};
        }
    }
}