using System.Threading.Tasks;
using Newtonsoft.Json;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.EventFlow.Tasks;
using OSS.EventFlow.Tasks.Mos;

namespace OSS.EventFlow.Tests
{
    public class NotifyTask : BaseTask<NotifyMsg, ResultMo>
    {
        public NotifyTask()
        {
            SetContinueRetry(9);
            SetIntervalRetry(2, SaveTaskContext);
        }

        public Task SaveTaskContext(TaskContext<NotifyMsg> context)
        {
            LogUtil.Info("临时保存任务相关请求信息：" + JsonConvert.SerializeObject(context));
            return Task.CompletedTask;
        }

        protected override Task<ResultMo> Do(NotifyMsg req)
        {
            LogUtil.Info("已经开始在执行");
            // 手动表示执行出错！
            return Task.FromResult(new ResultMo((int) EventFlowResult.Failed));
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


    public class NotifyMsg
    {
        public string name { get; set; }

        public string msg { get; set; }

        public string num { get; set; }
    }
}
