using System.Threading.Tasks;
using OSS.EventFlow.Activity;
using OSS.EventFlow.Connector;
using OSS.EventFlow.Mos;
using OSS.Tools.Log;

namespace OSS.TaskFlow.Tests.FlowItems
{
    public class StockActivity : BaseActivity<StockContext>
    {
        protected override Task<bool> Executing(StockContext data)
        {
            LogHelper.Info("分流-2.库存保存");
            return Task.FromResult(true);
        }
    }

    public class StockContext : TestContext<string>
    {

    }

    public class StockConnector : BaseConnector<PayContext, StockContext>
    {
        protected override StockContext Convert(PayContext inContextData)
        {
            return new StockContext() { id = inContextData.id };
        }
    }
}