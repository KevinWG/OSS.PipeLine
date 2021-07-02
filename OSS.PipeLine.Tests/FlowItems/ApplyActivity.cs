using System.Threading.Tasks;
using OSS.Tools.Log;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class ApplyActivity : BaseEffectActivity<ApplyContext, long>
    {
        public ApplyActivity():base("ApplyActivity")
        {
        }
        
        protected override Task<TrafficSignal<long>> Executing(ApplyContext para)
        {
            LogHelper.Info($"发起 [{para.name}] 采购申请");
            return Task.FromResult(new TrafficSignal<long>(100000001L));
        }
    }

    public class ApplyContext 
    {
        public string name { get; set; }
    }
}