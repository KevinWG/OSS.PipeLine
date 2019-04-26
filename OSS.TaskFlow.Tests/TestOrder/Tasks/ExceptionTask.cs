using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Tasks;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tests.TestOrder.Tasks
{
    public class ExceptionTask : BaseTask<OrderInfo,ResultMo>
    {
        protected override Task<ResultMo> Do(TaskContext<OrderInfo> context)
        {
             throw new Exception("执行失败！");
        }
    }
    
 
}
