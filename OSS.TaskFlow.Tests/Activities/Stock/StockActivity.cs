using System.Threading.Tasks;
using OSS.EventFlow.Activity;
using OSS.Tools.Log;

namespace OSS.TaskFlow.Tests.Activities.Stock
{
    public class StockActivity : BaseActivity<StockContext>
    {
        protected override Task<bool> Execute(StockContext data)
        {
            LogHelper.Info("库存保存");
            return Task.FromResult(true);
        }
    }
}