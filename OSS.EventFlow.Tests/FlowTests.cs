using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.EventFlow.Tests.TestOrder;

namespace OSS.EventFlow.Tests
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


            var flow=new OrderFlow();

            flow.Entry();


        }




    }
}
