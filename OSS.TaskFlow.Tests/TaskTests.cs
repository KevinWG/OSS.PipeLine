using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OSS.Common.Plugs.LogPlug;
using OSS.EventTask.Mos;
using OSS.TaskFlow.Tasks.Mos;
using OSS.TaskFlow.Tests.TestOrder;
using OSS.TaskFlow.Tests.TestOrder.Tasks;

namespace OSS.TaskFlow.Tests
{
    [TestClass]
    public class TaskTests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var msg = new OrderInfo()
            {
                order_name = "≤‚ ‘∂©µ•!",
                id = 123456,
                price = 10.23M
            };

            var taskContext = new TaskContext<OrderInfo>();
            var task = new OrderNotifyTask();

            //task.SetContinueRetry(9);
            //task.SetIntervalRetry(2, SaveTaskContext);

            await task.Process(taskContext);
        }




     
    }
}
