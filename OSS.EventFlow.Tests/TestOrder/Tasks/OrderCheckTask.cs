using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Plugs.LogPlug;
using OSS.EventFlow.Tasks;
using OSS.EventFlow.Tasks.Mos;

namespace OSS.EventFlow.Tests.TestOrder.Tasks
{
    public class OrderCheckTask : BaseTask<OrderInfo, ResultMo>
    {
        protected override async Task<ResultMo> Do(TaskContext<OrderInfo> context)
        {
            LogUtil.Info("执行确认订单！");
            if (context.Body.status <= 0)
                return new ResultMo(ResultTypes.ObjectNull, "当前订单状态异常！");

            //  todo 修改订单状态为已确认
            return new ResultMo();
        }

        protected override Task Revert(TaskContext<OrderInfo> context)
        {
            LogUtil.Info("执行确认订单回退！");
            return Task.CompletedTask;
        }

        protected override Task Failed(TaskContext<OrderInfo> context)
        {
            LogUtil.Info("执行确认订单失败！");
            return Task.CompletedTask;
        }
    }


}
