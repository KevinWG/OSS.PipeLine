using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.Pipeline.Tests.FlowItems;

namespace OSS.Pipeline.Tests
{
    [TestClass]
    public class BuyFlowTests
    {
        public readonly ApplyActivity     ApplyActivity = new ApplyActivity();
        public readonly AutoAuditActivity AuditActivity = new AutoAuditActivity();

        public readonly PayActivity PayActivity = new PayActivity();

        public readonly PayGateway PayGateway = new PayGateway();

        public readonly StockConnector StockConnector = new StockConnector();
        public readonly StockActivity  StockActivity  = new StockActivity();

        public readonly PayEmailConnector EmailConnector = new PayEmailConnector();
        public readonly SendEmailActivity EmailActivity  = new SendEmailActivity();



        private EndGateway _endNode = new EndGateway();

        //  构造函数内定义流体关联
        public BuyFlowTests()
        {


            ApplyActivity
            .Append(AuditActivity)

            .Append(PayActivity)
            .Append(PayGateway);

            // 网关分支 - 发送邮件分支
            PayGateway
            .Append(EmailConnector)
            .Append(EmailActivity)
            .Append(_endNode);

            // 网关分支- 入库分支
            PayGateway
            .Append(StockConnector)
            .Append(StockActivity)
            .Append(_endNode);


        }

        [TestMethod]
        public async Task FlowTest()
        {
            await ApplyActivity.Execute(new ApplyContext()
            {
                name = "冰箱"
            });
            await Task.Delay(1000);
            await PayActivity.Execute(new PayContext()
            {
                count = 10,
                money = 10000
            });

            await Task.Delay(2000);
        }

        [TestMethod]
        public void RouteTest()
        {
            var TestPipeline = new Pipeline<ApplyContext, Empty>("test-flow", ApplyActivity, _endNode);

            var route = TestPipeline.ToRoute();
            Assert.IsTrue(route != null);
        }
    }

}
