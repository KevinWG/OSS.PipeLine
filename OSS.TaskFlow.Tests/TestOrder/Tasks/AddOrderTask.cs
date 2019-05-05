using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventTask;
using OSS.EventTask.Mos;

namespace OSS.TaskFlow.Tests.TestOrder.Tasks
{
    public class AddOrderTask : BaseStandTask<OrderInfo, ResultIdMo>
    {
        protected override async Task<ResultIdMo> Do(TaskContext<OrderInfo> context)
        {
            return new ResultIdMo("1");
        }
    }


}
