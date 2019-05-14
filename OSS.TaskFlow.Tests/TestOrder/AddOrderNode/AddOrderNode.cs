using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventNode;
using OSS.EventTask.Interfaces;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Tasks;

namespace OSS.TaskFlow.Tests.TestOrder.Nodes
{
    /// <summary>
    ///   添加订单节点
    /// </summary>
    public class AddOrderNode : BaseNode<AddOrderReq, ResultIdMo>
    {
        private static List<IBaseTask<AddOrderReq>> list; // = new List<IBaseTask<ExcuteReq<AddOrderReq>>>(){ new AddOrderTask() };


        static AddOrderNode()
        {
            list = new List<IBaseTask< AddOrderReq>> ()
            {
                new CouponUseTask(),
                new PriceComputeTask(),
                new StockUseTask(),
                new InsertOrderTask()
            };
        }


        // 获取所有执行任务
        protected override async Task<IList<IBaseTask<AddOrderReq>>> GetTasks()
        {
            return list;
        }
    }
}
