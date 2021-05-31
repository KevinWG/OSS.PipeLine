using System.Threading.Tasks;
using OSS.EventFlow.Activity;
using OSS.Tools.Log;

namespace OSS.TaskFlow.Tests.FlowItems
{
    public class PayActivity : BaseActivity<PayContext>
    {
        public PayActivity()
        {
            PipeCode = "PayActivity";
        }

        protected override Task<bool> Executing(PayContext data)
        {
            LogHelper.Info("发起支付处理");
            return Task.FromResult(true);
        }
    }

    public class PayContext : TestContext<string>
    {

    }
}