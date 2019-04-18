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
            var msg = new NotifyMsg();
            msg.msg = "≤‚ ‘œ˚œ¢!";
            msg.num = "123456";
            msg.name = "Hi,World";

            var taskContext = new TaskContext();

            var task = new NotifyTask(new NotifySaver());
            var res = await task.Process(taskContext, msg);
        }
    }
}
