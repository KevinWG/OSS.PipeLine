using System.Threading.Tasks;
using OSS.Tools.Log;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class PayActivity : BasePassiveActivity<PayContext, bool>
    {
        public PayActivity():base("PayActivity")
        {
        }


        protected override Task<TrafficSignal<bool>> Executing(PayContext para)
        {
            LogHelper.Info($"通过{PipeCode} 支付动作执行,数量：{para.count}，金额：{para.money}）");
            return Task.FromResult(new TrafficSignal<bool>(true));
        }
    }

    public class PayContext 
    {
        public int    count  { get; set; }
        public decimal money { get; set; }
    }
}