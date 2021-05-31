using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.EventFlow;
using OSS.TaskFlow.Tests.FlowItems;

namespace OSS.TaskFlow.Tests
{
    [TestClass]
    public class BuyFlowTests
    {
        public readonly ApplyActivity ApplyActivity = new ApplyActivity();
        public readonly AuditActivity AuditActivity = new AuditActivity();

        public readonly PayActivity PayActivity = new PayActivity();

        public readonly PayGateway PayGateway = new PayGateway();

        public readonly StockConnector StockConnector = new StockConnector();
        public readonly StockActivity  StockActivity  = new StockActivity();

        public readonly PayEmailConnector EmailConnector = new PayEmailConnector();
        public readonly SendEmailActivity EmailActivity  = new SendEmailActivity();


        public readonly EventFlow<ApplyContext, StockContext> Flow;
        //  构造函数内定义流体关联
        public BuyFlowTests()
        {
            ApplyActivity
            .Append(AuditActivity)
            .AppendConvert(applyContext => new PayContext() {id = applyContext.id})// 表达式方式的转化器
            .Append(PayActivity)
            .Append(PayGateway);

            // 网关分支 - 发送邮件分支
            PayGateway.AddBranchPipe(EmailConnector)
            .Append(EmailActivity);

            // 网关分支- 入库分支
            PayGateway.AddBranchPipe(StockConnector)
            .Append(StockActivity);
            //.Append(后续事件)

            Flow = ApplyActivity.AsFlowStartAndEndWith(StockActivity);

        }


        [TestMethod]
        public async Task FlowTest()
        {
            await Flow.Start(new ApplyContext()
            {
                id = "test_business_id"
            });
            await Task.Delay(2000);
        }

        [TestMethod]
        public void RouteTest()
        {
            var route = Flow.ToRoute();
        }
    }
}
