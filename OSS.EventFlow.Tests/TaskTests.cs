using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.EventFlow.Tasks.Mos;

namespace OSS.EventFlow.Tests
{
    [TestClass]
    public class TaskTests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var msg = new NotifyMsg
            {
                msg = "≤‚ ‘œ˚œ¢!",
                num = "123456",
                name = "Hi,World"
            };

            var taskContext = new TaskContext<NotifyMsg>(msg);

            var task = new NotifyTask();
            var res = await task.Process(taskContext);
        }
    }
}
