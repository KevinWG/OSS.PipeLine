using System.Threading.Tasks;
using OSS.Pipeline.Activity;
using OSS.Tools.Log;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class ApplyActivity : BaseEffectActivity<ApplyContext, long>
    {
        public ApplyActivity()
        {
            PipeCode = "ApplyActivity";
        }
        
        protected override Task<(bool is_ok, long result)> Executing(ApplyContext contextData)
        {
            LogHelper.Info($"发起 [{contextData.name}] 采购申请");
            return Task.FromResult((true,100000001L));
        }
    }

    public class ApplyContext 
    {
        public string name { get; set; }
    }
}