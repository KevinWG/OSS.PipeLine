using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.Resp;
using OSS.EventNode;
using OSS.EventNode.MetaMos;
using OSS.EventTask.Interfaces;
using OSS.EventTask.Mos;
using OSS.TaskFlow.Tests.TestNodes.AddNode.Tasks;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;

namespace OSS.TaskFlow.Tests.TestNodes.AddNode
{
    /// <summary>
    ///   添加订单节点
    /// </summary>
    public class AddNode : BaseNode<AddOrderReq, IdResp>
    {
        // 获取所有执行任务
        private static List<IEventTask<AddOrderReq>> list;
        protected override Task<List<IEventTask<AddOrderReq>>> GetTasks() => Task.FromResult(list);
        public AddNode()
        {
            var couponTask = new CouponUseTask();
            couponTask.TaskMeta.WithNodeMeta(NodeMeta);

            var priceTask = new PriceComputeTask();
            priceTask.TaskMeta.WithNodeMeta(NodeMeta);
            
            var stockTask = new StockUseTask();
            stockTask.TaskMeta.WithNodeMeta(NodeMeta);

            var insertTask = new InsertOrderTask();
            insertTask.TaskMeta.WithNodeMeta(NodeMeta);
            
            list = new List<IEventTask<AddOrderReq>>(){
                couponTask,priceTask,insertTask,stockTask
            };
        }
        
        protected override NodeMeta GetDefaultConfig()
        {
            return new NodeMeta()
            {
                flow_id = "Order_Flow",
                node_alias = "添加订单",
                owner_type = OwnerType.Node,
                Process_type = NodeProcessType.Serial,

                node_id = "AddOrderNode"
            };
        }
    }
}
