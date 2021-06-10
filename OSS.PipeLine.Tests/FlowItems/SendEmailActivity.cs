
using OSS.Tools.Log;
using System.Threading.Tasks;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class SendEmailActivity : BaseActivity<SendEmailContext>
    {
        public SendEmailActivity()
        {
            PipeCode = "SendEmailActivity";
        }
       
        protected override Task<TrafficSignal> Executing(SendEmailContext data)
        {
            LogHelper.Info("分流-1.邮件发送，内容："+data.body);
            return Task.FromResult(TrafficSignal.Green_Pass);
        }
    }

    public class SendEmailContext 
    {
        public string body { get; set; }
    }

    public class PayEmailConnector : BaseMsgConverter<PayContext, SendEmailContext>
    {
        public PayEmailConnector()
        {
            PipeCode = "PayEmailConnector";
        }
        protected override SendEmailContext Convert(PayContext inContextData)
        {
            // ......
            return new SendEmailContext() { body = $" 您成功支付了订单，总额：{inContextData.money}" };
        }
    }
}
