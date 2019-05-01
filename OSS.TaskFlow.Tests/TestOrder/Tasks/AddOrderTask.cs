﻿using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.EventTask.Mos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tests.TestOrder.Tasks
{
    public class AddOrderTask : EventTask.BaseStandTask<OrderInfo, ResultIdMo>
    {
     
        protected override async Task<ResultIdMo> Do(TaskContext context, TaskReqData<OrderInfo> data)
        {
            LogUtil.Info("添加订单！");

            //  todo 修改订单状态为已确认
            return new ResultIdMo();
        }
    }


}
