using OSS.EventFlow.Activity;
using System.Threading.Tasks;
using OSS.EventFlow.Connector;
using OSS.EventFlow.Mos;
using OSS.Tools.Log;

namespace OSS.TaskFlow.Tests.FlowItems
{
    public class SendEmailActivity : BaseActivity<SendEmailContext>
    {
        public SendEmailActivity()
        {
                pipe_code = "SendEmailActivity";
        }

        protected override Task<bool> Executing(SendEmailContext data)
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
                pipe_code = "PayEmailConnector";
        }

        protected override SendEmailContext Convert(PayContext inContextData)
        {
            // ......
            return new SendEmailContext() { id = inContextData.id };
        }
    }
}
