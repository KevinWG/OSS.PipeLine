using System;
using System.Threading.Tasks;
using OSS.Common.Resp;
using OSS.EventTask;
using OSS.EventTask.Extention;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;

namespace OSS.TaskFlow.Tests.TestNodes.AddNode.Tasks
{
    public class InsertOrderTask : BaseTask<AddOrderReq, IdResp>
    {
        protected override TaskMeta GetDefaultConfig()
        {
            return new TaskMeta
            {
                task_id = "InsertOrderTask",
                task_alias = "添加订单！",
                loop_times = 3,
                node_action = NodeResultAction.RevrtAllOnFailed
            };
        }

        /// <inheritdoc />
        protected override async Task<DoResp<IdResp>> Do(AddOrderReq data, int loopTimes, int triedTimes)
        {
            throw new ArgumentNullException("sssssss");
            return new DoResp<IdResp>()
            {
                resp = new IdResp("1000"),
                run_status = TaskRunStatus.RunCompoleted
            };
        }
    }


}
