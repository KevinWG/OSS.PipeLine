﻿
using OSS.Tools.Log;
using System.Threading.Tasks;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class SendEmailActivity : BaseActivity<SendEmailContext>
    {
        public SendEmailActivity():base("SendEmailActivity")
        {
        }
       
        protected override Task<TrafficSignal> Executing(SendEmailContext data)
        {
            LogHelper.Info($"分流-1（{PipeCode}）邮件发送，内容：" + data.body);
            return Task.FromResult(TrafficSignal.GreenSignal);
        }
    }

    public class SendEmailContext 
    {
        public string body { get; set; }
    }

    public class PayEmailConnector : BaseMsgConverter<PayContext, SendEmailContext>
    {
        public PayEmailConnector():base("PayEmailConnector")
        {
        }
        protected override SendEmailContext Convert(PayContext inContextData)
        {
            // ......
            return new SendEmailContext() { body = $" 您成功支付了订单，总额：{inContextData.money}" };
        }
    }
}
