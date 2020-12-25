using System.Threading.Tasks;
using OSS.EventFlow.Activity;
using OSS.Tools.Log;

namespace OSS.TaskFlow.Tests.Activities.Apply
{
    public class ApplyActivity : BaseActivity<ApplyContext>
    {
        protected override Task<bool> Execute(ApplyContext data)
        {
            LogHelper.Info("这里刚才发生了一个采购申请"); 
            return Task.FromResult(true);
        }
    }
}