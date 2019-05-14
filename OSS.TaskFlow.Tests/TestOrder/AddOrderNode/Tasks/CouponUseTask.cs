using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;

namespace OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Tasks
{
    public class CouponUseTask:BaseAddOrderTask
    {
        protected override async Task<DoResponse<ResultIdMo>> Do( AddOrderReq req)
        {
           return new DoResponse<ResultIdMo>()
           {
               run_status = TaskRunStatus.RunCompoleted,
               resp = new ResultIdMo()
           };
        }
    }
}
