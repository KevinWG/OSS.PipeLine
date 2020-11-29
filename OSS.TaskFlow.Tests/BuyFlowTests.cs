using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.TaskFlow.Tests.FlowContexts;

namespace OSS.TaskFlow.Tests
{
    [TestClass]
    public class BuyFlowTests
    {
        private static readonly BuyFlow bFlow = new BuyFlow();

        [TestMethod]
        public void FlowTest()
        {
            bFlow.Trigger(new ApplyContext());
        }

    }
}
