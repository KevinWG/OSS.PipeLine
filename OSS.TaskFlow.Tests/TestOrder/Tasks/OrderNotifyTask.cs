using System.Threading;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.TaskFlow.Tasks;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tests.TestOrder.Tasks
{
    public class OrderNotifyTask : BaseTask<OrderInfo,ResultMo>
    {
        protected override Task<ResultMo> Do(TaskContext<OrderInfo> context)
        {
            Task.Delay(2000);
            LogUtil.Info("执行通知！");
            // 手动表示执行出错！
            return Task.FromResult(new ResultMo());
        }
    }
    
    public class NotifyMsg
    {
        public string name { get; set; }

        public string msg { get; set; }

        public string num { get; set; }
    }
}
