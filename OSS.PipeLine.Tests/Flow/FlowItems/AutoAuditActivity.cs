using System.Threading.Tasks;
using OSS.Tools.Log;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class AutoAuditActivity : BaseEffectActivity<long,bool>
    {
        public AutoAuditActivity():base("AuditActivity")
        {
        }
        
     

        protected override Task<TrafficSignal<bool>> Executing(long id)
        {
            LogHelper.Info($"通过{PipeCode} 自动审核通过申请（编号：{id}）");
            return Task.FromResult(new TrafficSignal<bool>(true));
        }
    }
}
