using System.Threading.Tasks;
using OSS.EventFlow.Activity;
using OSS.TaskFlow.Tests.FlowNodes.Apply;
using OSS.Tools.Log;

namespace OSS.TaskFlow.Tests.FlowNodes.Audit
{
    public class AutoAuditActivity : BaseActivity<ApplyContext>
    {
        protected override Task<bool> Execute(ApplyContext data)
        {
            LogHelper.Info("管理员审核通过");
            return Task.FromResult(true);
        }
    }
}
    