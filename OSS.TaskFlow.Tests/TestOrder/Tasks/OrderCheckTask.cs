using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.TaskFlow.Tasks;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tests.TestOrder.Tasks
{
    public class OrderCheckReq
    {
    }


    public class OrderCheckTask : BaseStandTask<OrderCheckReq, ResultMo>
    {
        protected override Task Revert(TaskContext context, TaskReqData<OrderCheckReq> data)
        {
            LogUtil.Info("执行确认订单回退！");
            return Task.CompletedTask;
        }

        protected override Task Failed(TaskContext context, TaskReqData<OrderCheckReq> data)
        {
            LogUtil.Info("执行确认订单失败！");
            return Task.CompletedTask;
        }

        protected override async Task<ResultMo> Do(TaskContext context, TaskReqData<OrderCheckReq> data)
        {
            LogUtil.Info("执行确认订单！");
            //if (context.Body.status <= 0)
            //    return new TaskResultMo(TaskResultType.Failed, ResultTypes.ObjectNull, "当前订单状态异常！");

            //  todo 修改订单状态为已确认
            return new ResultMo();
        }
    }


}
