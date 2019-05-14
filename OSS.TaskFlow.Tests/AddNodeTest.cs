using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;
using OSS.TaskFlow.Tests.TestOrder.Nodes;

namespace OSS.TaskFlow.Tests
{
    [TestClass]
    public class AddNodeTest
    {
        [TestMethod]
        public async Task AddTest()
        {
            //var orderInfo = new OrderInfo();
            var addReq = new AddOrderReq
            {
                coupon_id = "coupon100",
                title = "≤‚ ‘∂©µ•",
                source_ids = "s_1"
            };

          
            var addNode = new AddOrderNode();
            var id = await addNode.Process(addReq);
        }
    }
}
