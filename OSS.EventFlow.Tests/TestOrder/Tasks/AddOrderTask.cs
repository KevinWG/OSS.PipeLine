using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.EventFlow.Tasks;
using OSS.EventFlow.Tasks.Mos;

namespace OSS.EventFlow.Tests.TestOrder.Tasks
{
    public class AddOrderTask : BaseTask<OrderInfo>
    {
        protected override async Task<ResultMo> Do(TaskContext<OrderInfo> context)
        {
            LogUtil.Info("添加订单！");

            //  todo 修改订单状态为已确认
            return new ResultMo();
        }
      
    }


}
