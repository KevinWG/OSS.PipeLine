using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.EventTask;
using OSS.EventTask.Extension;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;

namespace OSS.TaskFlow.Tests.TestNodes.AddNode.Tasks
{
    public class CouponUseTask : EventTask<AddOrderReq, Resp>
    {
        public CouponUseTask() : base(new TaskMeta()
        {
            loop_times = 3,
            task_id = "CouponUseTask",
            task_alias = "使用优惠券"
        })
        {
        }

        protected override Task<DoResp<Resp>> Do(AddOrderReq data, int loopTimes, int triedTimes)
        {
            var resp = new DoResp<Resp>
            {
                run_status = TaskRunStatus.RunCompleted,
                resp = new Resp()
            };
            return Task.FromResult(resp);
        }
    }
}
