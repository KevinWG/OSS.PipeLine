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
        // 获取所有执行任务
        private static List<IBaseTask<AddOrderReq>> list;
        protected override Task<List<IBaseTask<AddOrderReq>>> GetTasks() => Task.FromResult(list);
        public AddOrderNode()
        {
            var couponTask = new CouponUseTask();
            couponTask.TaskMeta.WithNodeMeta(NodeMeta);

            var priceTask = new PriceComputeTask();
            priceTask.TaskMeta.WithNodeMeta(NodeMeta);
            
            var stockTask = new StockUseTask();
            stockTask.TaskMeta.WithNodeMeta(NodeMeta);

            var insertTask = new InsertOrderTask();
            insertTask.TaskMeta.WithNodeMeta(NodeMeta);
            
            list = new List<IBaseTask<AddOrderReq>>(){
                couponTask,priceTask,stockTask,insertTask
            };
        }
        
        protected override NodeMeta GetDefaultConfig()
        {
            return new NodeMeta()
            {
                flow_id = "Order_Flow",
                node_alias = "添加订单",
                owner_type = OwnerType.Node,
                Process_type = NodeProcessType.Sequence,

                node_id = "AddOrderNode"
            };
        }
    }
}
