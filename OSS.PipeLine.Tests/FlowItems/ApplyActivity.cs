using System.Threading.Tasks;
using OSS.PipeLine.Activity;
using OSS.Tools.Log;

namespace OSS.PipeLine.Tests.FlowItems
{
    public class ApplyActivity : BaseActivity<ApplyContext>
    {
        public ApplyActivity()
        {
            PipeCode = "ApplyActivity";
        }
        protected override Task<bool> Executing(ApplyContext data)
        {
            LogHelper.Info("这里刚才发生了一个采购申请");
            return Task.FromResult(true);
        }
    }

    public class ApplyContext : TestContext<string>
    {
    }
}