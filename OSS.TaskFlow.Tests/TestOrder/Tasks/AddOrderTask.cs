using System.Threading.Tasks;
using OSS.Common.Plugs.LogPlug;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tests.TestOrder.Tasks
{
    public class AddOrderTask : TaskFlow.Tasks.BaseTask<OrderInfo, TaskResultMo>
    {
        protected override async Task<TaskResultMo> Do(TaskContext<OrderInfo> context)
        {
            LogUtil.Info("添加订单！");

            //  todo 修改订单状态为已确认
            return new TaskResultMo();
        }
      
    }


}
