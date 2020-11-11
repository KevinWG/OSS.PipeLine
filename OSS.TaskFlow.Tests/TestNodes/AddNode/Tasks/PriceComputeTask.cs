using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.EventTask;
using OSS.EventTask.Extension;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;

namespace OSS.TaskFlow.Tests.TestNodes.AddNode.Tasks
{
    public class PriceComputeTask:EventTask<AddOrderReq,Resp>
    {
       
        public PriceComputeTask() : base(new TaskMeta
        {
            task_id = "PriceComputeTask",
            task_alias = "价格计算！",
            loop_times = 3,
            failed_effect = FailedEffect.FailedSelf
        })
        {

        }

        protected override async Task<DoResp<Resp>> Do(AddOrderReq data, int loopTimes, int triedTimes)
        {
            return new DoResp<Resp>()
            {
                run_status = TaskRunStatus.RunCompleted,
                resp = new Resp()
            };
        }
    }
}
