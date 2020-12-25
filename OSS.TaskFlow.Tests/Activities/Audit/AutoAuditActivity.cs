using System.Threading.Tasks;
using OSS.EventFlow.Activity;
using OSS.TaskFlow.Tests.Activities.Apply;
using OSS.Tools.Log;

namespace OSS.TaskFlow.Tests.Activities.Audit
{
    public class AutoAuditActivity : BaseActivity<ApplyContext>
    {
        protected override Task<bool> Executing(ApplyContext data)
        {
            LogHelper.Info("管理员审核通过");
            return Task.FromResult(true);
        }
    }
}
    