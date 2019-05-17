﻿using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventTask;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;

namespace OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Tasks
{
    public class PriceComputeTask:BaseTask<AddOrderReq,ResultMo>
    {
        protected override TaskMeta GetDefaultConfig()
        {
            return new TaskMeta
            {
                task_id = "PriceComputeTask",
                task_alias = "价格计算！",
                loop_times = 3,
                node_action = NodeResultAction.FailedOnFailed
            };
        }
        protected override async Task<DoResp<ResultMo>> Do(AddOrderReq data, int loopTimes, int triedTimes)
        {
            return new DoResp<ResultMo>()
            {
                run_status = TaskRunStatus.RunCompoleted,
                resp = new ResultMo()
            };
        }
    }
}
