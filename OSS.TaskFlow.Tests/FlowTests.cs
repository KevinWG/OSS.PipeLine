using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.TaskFlow.Tests.TestOrder;

namespace OSS.TaskFlow.Tests
{
    [TestClass]
    public class FlowTests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var order = new OrderInfo()
            {
                order_name = "≤‚ ‘∂©µ•!",
                id = 123456,
                price = 10.23M
            };

            //var flow=new OrderFlow();
            //flow.Entry();
        }




    }
}
