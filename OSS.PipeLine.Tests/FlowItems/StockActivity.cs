using OSS.Pipeline.Activity;
using OSS.Pipeline.Connector;
using OSS.Tools.Log;
using System.Threading.Tasks;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class StockActivity : BaseEffectActivity<StockContext,bool>
    {
        public StockActivity()
        {
            PipeCode = "StockActivity";
        }
        
        protected override Task<bool> Executing(StockContext contextData, ref bool isBlocked)
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
        public StockConnector()
        {
            PipeCode = "StockConnector";
        }

        protected override StockContext Convert(PayContext inContextData)
        {
            return new StockContext() { id = inContextData.id };
        }
    }
}