using OSS.Pipeline.Activity;
using OSS.Tools.Log;
using System.Threading.Tasks;
using OSS.Pipeline.Msg;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class StockActivity : BaseActivity<StockContext>
    {
        public StockActivity()
        {
            PipeCode = "StockActivity";
        }

        protected override Task<bool> Executing(StockContext data)
        {
            LogHelper.Info("分流-2.增加库存，数量：" + data.count);
            return Task.FromResult(true);
        }
    }

    public class StockContext
    {
        public int count { get; set; }
    }

    public class StockConnector : BaseMsgConverter<PayContext, StockContext>
    {
        public StockConnector()
        {
            PipeCode = "StockConnector";
        }

        protected override StockContext Convert(PayContext inContextData)
        {
            return new StockContext() {count = inContextData.count};
        }
    }
}