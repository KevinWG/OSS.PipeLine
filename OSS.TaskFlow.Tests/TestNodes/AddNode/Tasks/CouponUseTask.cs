using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventTask;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;

namespace OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Tasks
{
    public class CouponUseTask:BaseTask<AddOrderReq,ResultMo>
    {
        protected override TaskMeta GetDefaultConfig()
        {
            return new TaskMeta
            {
                loop_times = 3,
                task_id = "CouponUseTask",
                task_alias = "使用优惠券" ,
                node_action = NodeResultAction.FailedOnFailed
            };
        }

        protected override Task<DoResp<ResultMo>> Do(AddOrderReq data, int loopTimes, int triedTimes)
        {
            var resp = new DoResp<ResultMo>
            {
                run_status = TaskRunStatus.RunCompoleted,
                resp = new ResultMo()
            };
            return Task.FromResult(resp);
        }
    }
}
