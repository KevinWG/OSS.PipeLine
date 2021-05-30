using System.Threading.Tasks;
using OSS.EventFlow.Activity;
using OSS.EventFlow.Mos;
using OSS.Tools.Log;

namespace OSS.TaskFlow.Tests.FlowItems
{
    public class AuditActivity : BaseActivity<ApplyContext>
    {
        public AuditActivity()
        {
            pipe_meta = new PipeMeta()
            {
                pipe_code = "AuditActivity"
            };
        }

        protected override Task<bool> Executing(ApplyContext data)
        {
            LogHelper.Info("管理员审核通过");
            return Task.FromResult(true);
        }
    }
}
    