using System.Threading.Tasks;
using OSS.Tools.Log;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class PayActivity : BaseFuncActivity<PayContext, bool>
    {
        public PayActivity()
        {
            PipeCode = "PayActivity";
        }

        protected override Task<(bool is_ok, bool result)> Executing(PayContext conext)
        {
            LogHelper.Info($"支付动作执行,数量：{conext.count}，金额：{conext.money}）");
            return Task.FromResult((true,true));
        }

      
    }

    public class PayContext 
    {
        public int    count  { get; set; }
        public decimal money { get; set; }
    }
}