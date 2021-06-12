using System.Threading.Tasks;
using OSS.Tools.Log;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class ApplyActivity : BaseEffectActivity<ApplyContext, long>
    {
        public ApplyActivity()
        {
            PipeCode = "ApplyActivity";
        }
        
        protected override Task<(TrafficSignal traffic_signal, long result)> Executing(ApplyContext para)
        {
            LogHelper.Info($"发起 [{para.name}] 采购申请");
            return Task.FromResult((new TrafficSignal(SignalFlag.Green_Pass), 100000001L));
        }
    }

    public class ApplyContext 
    {
        public string name { get; set; }
    }
}