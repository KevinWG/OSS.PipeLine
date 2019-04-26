﻿using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tests.TestOrder.Tasks
{
    public class AddOrderTask : TaskFlow.Tasks.BaseTask<OrderInfo, ResultIdMo>
    {
        protected override async Task<ResultIdMo> Do(TaskContext<OrderInfo> context)
        {
            LogUtil.Info("添加订单！");

            //  todo 修改订单状态为已确认
            return new ResultIdMo();
        }
      
    }


}