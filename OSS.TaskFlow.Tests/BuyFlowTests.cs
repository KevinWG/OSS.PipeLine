using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.TaskFlow.Tests.Activities.Apply;

namespace OSS.TaskFlow.Tests
{
    [TestClass]
    public class BuyFlowTests
    {
        private static readonly AppLifeFlow bFlow = new AppLifeFlow();

        [TestMethod]
        public async Task FlowTest()
        {
            await bFlow.Start(new ApplyContext()); 
        }

    }
}
