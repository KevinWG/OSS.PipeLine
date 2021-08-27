using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.Pipeline.Tests.FlowItems;

namespace OSS.Pipeline.Tests
{
    [TestClass]
    public class BuyFlowTests
    {
        private static readonly BuyFlow _flow = new BuyFlow();
        
        [TestMethod]
        public async Task FlowTest()
        {
            await _flow.ApplyActivity.Execute(new ApplyContext()
            {
                name = "冰箱"
            });

            // 延后一秒，假装有支付操作
            await Task.Delay(1000);

            await _flow.PayActivity.Execute(new PayContext()
            {
                count = 10,
                money = 10000
            });
            await Task.Delay(1000);// 等待异步日志执行完成
        }

        [TestMethod]
        public void RouteTest()
        {
            var route = _flow.ToRoute();
            Assert.IsTrue(route != null);
        }
    }

}
