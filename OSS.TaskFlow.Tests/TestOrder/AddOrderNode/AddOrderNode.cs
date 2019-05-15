using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventNode;
using OSS.EventNode.MetaMos;
using OSS.EventTask.Interfaces;
using OSS.EventTask.Mos;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Tasks;

namespace OSS.TaskFlow.Tests.TestOrder.AddOrderNode
{
    /// <summary>
    ///   添加订单节点
    /// </summary>
    public class AddOrderNode : BaseNode<AddOrderReq, ResultIdMo>
    {
        public AddOrderNode()
        {
        }

        // 获取所有执行任务
        private static List<IBaseTask<AddOrderReq>> list;
        protected override Task<List<IBaseTask<AddOrderReq>>> GetTasks() => Task.FromResult(list);

        static AddOrderNode()
        {
            list = new List<IBaseTask<AddOrderReq>>()
            {
                new CouponUseTask(),
                new PriceComputeTask(),
                new StockUseTask(),
                new InsertOrderTask()
            };
        }


        protected override NodeMeta GetDefaultConfig()
        {
            return new NodeMeta()
            {
                flow_id = "Order_Flow",
                node_alias = "添加订单",
                owner_type = OwnerType.Node,
                Process_type = NodeProcessType.Parallel,

                node_id = "AddOrderNode"
            };
        }
    }
}
