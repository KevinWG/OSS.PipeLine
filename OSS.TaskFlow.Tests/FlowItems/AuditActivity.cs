using System.Threading.Tasks;
using OSS.EventFlow.Activity;
using OSS.Tools.Log;

namespace OSS.TaskFlow.Tests.FlowItems
{
    public class AuditActivity : BaseActivity<ApplyContext>
    {
        protected override Task<bool> Executing(ApplyContext data)
        {
            LogHelper.Info("管理员审核通过");
            return Task.FromResult(true);
        }
    }
}
    