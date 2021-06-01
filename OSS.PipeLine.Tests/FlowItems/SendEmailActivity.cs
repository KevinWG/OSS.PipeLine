using OSS.Pipeline.Activity;
using OSS.Pipeline.Connector;
using OSS.Tools.Log;
using System.Threading.Tasks;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class SendEmailActivity : BaseEffectActivity<SendEmailContext, bool>
    {
        public SendEmailActivity()
        {
            PipeCode = "SendEmailActivity";
        }

        protected override Task<bool> Executing(SendEmailContext contextData, ref bool isBlocked)
        {
            LogHelper.Info("分流-1.邮件发送");
            return Task.FromResult(true);
        }
    }

    public class SendEmailContext : TestContext<string>
    {
    }

    public class PayEmailConnector : BaseConnector<PayContext, SendEmailContext>
    {
        public PayEmailConnector()
        {
            PipeCode = "PayEmailConnector";
        }
        protected override SendEmailContext Convert(PayContext inContextData)
        {
            // ......
            return new SendEmailContext() { id = inContextData.id };
        }
    }
}
