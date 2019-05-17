﻿using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventTask;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;

namespace OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Tasks
{
    public class InsertOrderTask : BaseTask<AddOrderReq, ResultIdMo>
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
        protected override async Task<DoResp<ResultIdMo>> Do(AddOrderReq data, int loopTimes, int triedTimes)
        {
            throw new ArgumentNullException("sssssss");
            return new DoResp<ResultIdMo>()
            {
                resp = new ResultIdMo("1000"),
                run_status = TaskRunStatus.RunCompoleted
            };
        }
    }


}
