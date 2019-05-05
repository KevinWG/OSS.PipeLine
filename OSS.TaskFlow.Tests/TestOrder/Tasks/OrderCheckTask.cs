using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventTask;
using OSS.EventTask.Mos;

namespace OSS.TaskFlow.Tests.TestOrder.Tasks
{
    public class OrderCheckReq
    {
    }


    public class OrderCheckTask : BaseStandTask<OrderCheckReq, ResultMo>
    {
        protected override Task<ResultMo> Do(TaskContext<OrderCheckReq> context)
        {
            return Task.FromResult(new ResultMo());
        }
    }


}
