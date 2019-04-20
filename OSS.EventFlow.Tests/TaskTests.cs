using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OSS.Common.Plugs.LogPlug;
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
                msg = "测试消息!",
                num = "123456",
                name = "Hi,World"
            };

            var taskContext = new TaskContext<NotifyMsg>(msg);
            var task = new NotifyTask();

            task.SetContinueRetry(9);
            task.SetIntervalRetry(2, SaveTaskContext);

            await task.Process(taskContext);
        }




        public Task SaveTaskContext(TaskContext<NotifyMsg> context)
        {
            LogUtil.Info("临时保存任务相关请求信息：" + JsonConvert.SerializeObject(context));
            return Task.CompletedTask;
        }
    }
}
