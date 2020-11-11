using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.EventTask;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;
using OSS.TaskFlow.Tests.TestNodes.AddNode.Tasks;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;

namespace OSS.TaskFlow.Tests.TestNodes.AddNode
{
    /// <summary>
    ///   添加订单节点
    /// </summary>
    public class AddNode : GroupEventTask<AddOrderReq, Resp>
    {
        // 获取所有执行任务
        private static List<IEventTask<AddOrderReq, Resp>> list;
        protected override Task<List<IEventTask<AddOrderReq, Resp>>> GetTasks() => Task.FromResult(list);
        public AddNode()
        {
            Meta = new GroupEventTaskMeta()
            {
                flow_id = "Order_Flow",
                group_alias = "添加订单",
                owner_type = OwnerType.Node,
                Process_type = GroupProcessType.Serial,

                group_id = "AddOrderNode"
            };

            var couponTask = new CouponUseTask();
            couponTask.Meta.WithGroupMeta(Meta);

            var priceTask = new PriceComputeTask();
            priceTask.Meta.WithGroupMeta(Meta);

            var stockTask = new StockUseTask();
            stockTask.Meta.WithGroupMeta(Meta);

            var insertTask = new InsertOrderTask();
            insertTask.Meta.WithGroupMeta(Meta);

            list = new List<IEventTask<AddOrderReq, Resp>>(){
                couponTask,priceTask,insertTask,stockTask
            };
        }

     
    }
}
