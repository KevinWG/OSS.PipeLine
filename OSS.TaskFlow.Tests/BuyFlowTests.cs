using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        public readonly PayGateway PayGateway = new PayGateway();

        public readonly StockConnector StockConnector = new StockConnector();
        public readonly StockActivity  StockActivity  = new StockActivity();

        public readonly PayEmailConnector EmailConnector = new PayEmailConnector();
        public readonly SendEmailActivity EmailActivity  = new SendEmailActivity();

        public BuyFlowTests()
        {
            ApplyActivity
            .Append(AutoAuditActivity)
            .Append(PayConnector)
            .Append(PayActivity)
            .Append(PayGateway);

            PayGateway.AddBranchPipe(StockConnector)
            .Append(StockActivity);

            PayGateway.AddBranchPipe(EmailConnector)
            .Append(EmailActivity);
        }


        [TestMethod]
        public async Task FlowTest()
        {
            await ApplyActivity.Start(new ApplyContext()
            {
                id="test_business_id"
            });
        }
    }
}
