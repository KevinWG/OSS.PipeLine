using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventTask;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;

namespace OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Tasks
{
    public class StockUseTask:BaseTask<AddOrderReq,ResultMo>
    {
        protected override TaskMeta GetDefaultConfig()
        {
            return new TaskMeta
            {
                task_id = "StockUseTask",
                task_alias = "扣减库存！",
                loop_times = 3,
                node_action = NodeResultAction.FailedOnFailed
            };
        }


        protected override async Task<DoResponse<ResultMo>> Do(AddOrderReq req, int loopTimes, int triedTimes)
        {
            //throw new ArgumentNullException("sssss");
            return new DoResponse<ResultMo>()
            {
                run_status = TaskRunStatus.RunCompoleted,
                resp = new ResultMo()
            };
        }
    }
}
