using System.Threading.Tasks;
using OSS.Pipeline.Activity;
using OSS.Tools.Log;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class AutoAuditActivity : BaseEffectActivity<long,bool>
    {
        public AutoAuditActivity()
        {
            PipeCode = "AuditActivity";
        }
        
        protected override Task<(bool is_ok, bool result)> Executing(long id)
        {
            LogHelper.Info($"自动审核通过申请（编号：{id}）");
            return Task.FromResult((true, true));
        }
        
    }
}
