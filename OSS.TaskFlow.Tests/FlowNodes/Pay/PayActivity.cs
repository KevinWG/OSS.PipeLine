using System.Threading.Tasks;
using OSS.EventFlow.Activity;
using OSS.Tools.Log;

namespace OSS.TaskFlow.Tests.FlowNodes.Pay
{
    public class PayActivity : BaseActivity<PayContext>
    {
        protected override Task<bool> Execute(PayContext data)
        {
            LogHelper.Info("发起支付处理");
            return Task.FromResult(true);
        }
    }
}