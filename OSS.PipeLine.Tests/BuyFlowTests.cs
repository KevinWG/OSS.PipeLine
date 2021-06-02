using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.Pipeline.Tests.FlowItems;

namespace OSS.Pipeline.Tests
{
    [TestClass]
    public class BuyFlowTests
    {
        public readonly ApplyActivity ApplyActivity = new ApplyActivity();
        public readonly AutoAuditActivity AuditActivity = new AutoAuditActivity();

        public readonly PayActivity PayActivity = new PayActivity();

        public readonly PayGateway PayGateway = new PayGateway();

        public readonly StockConnector StockConnector = new StockConnector();
        public readonly StockActivity StockActivity = new StockActivity();

        public readonly PayEmailConnector EmailConnector = new PayEmailConnector();
        public readonly SendEmailActivity EmailActivity = new SendEmailActivity();


        public readonly Pipeline<ApplyContext, EmptyContext> TestPipeline;
        //  构造函数内定义流体关联
        public BuyFlowTests()
        {
            var endActivity = new EmptyActivity();
            
            ApplyActivity
            .Append(AuditActivity)

            .Append(PayActivity)
            .Append(PayGateway);

            // 网关分支 - 发送邮件分支
            PayGateway
                .AddBranchPipe(EmailConnector)
                .Append(EmailActivity)
                .Append(endActivity);

            // 网关分支- 入库分支
            PayGateway
                .AddBranchPipe(StockConnector)
                .Append(StockActivity)
                .Append(endActivity); 
         
            // 流体对象
            TestPipeline = ApplyActivity.AsFlowStartAndEndWith(endActivity);
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
            // 获取当前的路由信息
            var route = TestPipeline.ToRoute();
            Assert.IsTrue(route != null);
        }
    }

}
