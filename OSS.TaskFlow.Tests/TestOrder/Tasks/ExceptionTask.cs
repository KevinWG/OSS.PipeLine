using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventTask;
using OSS.EventTask.Mos;

namespace OSS.TaskFlow.Tests.TestOrder.Tasks
{
    public class ExceptionTask : BaseStandTask<OrderInfo, ResultMo>
    {
        protected override Task<ResultMo> Do(TaskContext<OrderInfo> context)
        {
            return Task.FromResult(new ResultMo());
        }
    }
}
