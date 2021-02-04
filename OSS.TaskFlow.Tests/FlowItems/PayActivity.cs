using System.Threading.Tasks;
using OSS.EventFlow.Activity;
using OSS.EventFlow.Connector;
using OSS.EventFlow.Mos;
using OSS.Tools.Log;

namespace OSS.TaskFlow.Tests.FlowItems
{
    public class PayActivity : BaseActivity<PayContext>
    {
        protected override Task<bool> Executing(PayContext data)
        {
            LogHelper.Info("发起支付处理");
            return Task.FromResult(true);
        }
    }

    public class PayContext : TestContext<string>
    {

    }

    public class PayConnector : BaseConnector<ApplyContext, PayContext>
    {
        protected override PayContext Convert(ApplyContext inContextData)
        {
            return new PayContext() { id = inContextData.id };
        }
    }
}