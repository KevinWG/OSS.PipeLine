using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.EventFlow;
using OSS.TaskFlow.Tests.FlowItems;

namespace OSS.TaskFlow.Tests
{
    [TestClass]
    public class BuyFlowTests
    {
        public readonly ApplyActivity     ApplyActivity     = new ApplyActivity();
        public readonly AutoAuditActivity AutoAuditActivity = new AutoAuditActivity();

        public readonly PayConnector PayConnector = new PayConnector();
        public readonly PayActivity  PayActivity  = new PayActivity();

        public readonly StockConnector StockConnector = new StockConnector();
        public readonly StockActivity  StockActivity  = new StockActivity();

        public BuyFlowTests()
        {
            ApplyActivity
            .Append(AutoAuditActivity)
            .Append(PayConnector)
            .Append(PayActivity)
            .Append(StockConnector)
            .Append(StockActivity);
        }


        [TestMethod]
        public async Task FlowTest()
        {
            //var flow = new EventFlow<ApplyContext, StockContext>(ApplyActivity, StockActivity);
            await ApplyActivity.Start(new ApplyContext());
        }

    }
}
