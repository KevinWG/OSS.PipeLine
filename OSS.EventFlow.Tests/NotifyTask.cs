using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.EventFlow.Tasks;
using OSS.EventFlow.Tasks.Mos;

namespace OSS.EventFlow.Tests
{
    public class NotifyTask : BaseTask<NotifyMsg, ResultMo>
    {
        protected override Task<ResultMo> Do(TaskContext<NotifyMsg> context)
        {
            LogUtil.Info("已经开始在执行");
            // 手动表示执行出错！
            return Task.FromResult(new ResultMo((int) EventFlowResult.Failed));
        }

        protected override Task Revert(TaskContext<NotifyMsg> context)
        {
            LogUtil.Info("当次执行失败回退！");
            return Task.CompletedTask;
        }

        protected override Task Failed(TaskContext<NotifyMsg> context)
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
