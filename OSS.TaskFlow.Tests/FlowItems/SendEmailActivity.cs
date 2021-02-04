using OSS.EventFlow.Activity;
using System.Threading.Tasks;
using OSS.EventFlow.Connector;

namespace OSS.TaskFlow.Tests.FlowItems
{
    public class SendEmailActivity : BaseActivity<SendSmsContext>
    {
        protected override Task<bool> Executing(SendSmsContext data)
        {
            throw new System.NotImplementedException();
        }
    }

    public class SendSmsContext : TestContext<string>
    {
    }

    public class PaySmsConnector : BaseConnector<PayContext, SendSmsContext>
    {
        protected override SendSmsContext Convert(PayContext inContextData)
        {
            return new SendSmsContext() { id = inContextData.id };
        }
    }
}
