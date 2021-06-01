using System.Threading.Tasks;
using OSS.Pipeline.Activity;
using OSS.Tools.Log;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class AuditActivity : BaseActivity<ApplyContext>
    {
        public AuditActivity()
        {

            PipeCode = "AuditActivity";

        }

        protected override Task<bool> Executing(ApplyContext data)
        {
            LogHelper.Info("管理员审核通过");
            return Task.FromResult(true);
        }
    }
}
