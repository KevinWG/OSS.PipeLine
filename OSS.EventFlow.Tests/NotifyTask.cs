using System.Threading.Tasks;
using Newtonsoft.Json;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.EventFlow.Dispatcher;
using OSS.EventFlow.Tasks;
using OSS.EventFlow.Tasks.Mos;
using OSS.EventFlow.Tasks.Storage;

namespace OSS.EventFlow.Tests
{
    public class NotifyTask : BaseTask<NotifyMsg, ResultMo>
    {
        public NotifyTask(ITaskContextSaver<NotifyMsg> taskSaver) : base(taskSaver)
        {
            RetryConfig=new TaskRetryConfig()
            {
                DirectTimes = 3,
                IntervalTimes = 2
            };
        }

        protected override Task<ResultMo> Do(NotifyMsg req)
        {
            LogUtil.Info("已经开始在执行");
            return Task.FromResult(new ResultMo((int)EventFlowResult.Failed));
        }

        protected override Task Revert(NotifyMsg req)
        {
            LogUtil.Info("当次执行失败回退！");
            return Task.CompletedTask;
        }

        protected override Task Failed(NotifyMsg req)
        {
            LogUtil.Info("任务彻底执行失败！");
            return Task.CompletedTask;
        }
    }
    
    public class NotifySaver : ITaskContextSaver<NotifyMsg>
    {
        public Task SaveTaskContext(TaskContext context, NotifyMsg reqPara)
        {
             LogUtil.Info("临时保存任务相关请求信息："+JsonConvert.SerializeObject(context) + JsonConvert.SerializeObject(reqPara));
            return Task.CompletedTask;
        }
    }

    public class NotifyMsg
    {
        public string name { get; set; }
        public string msg { get; set; }

        public string num { get; set; }
    }
}
