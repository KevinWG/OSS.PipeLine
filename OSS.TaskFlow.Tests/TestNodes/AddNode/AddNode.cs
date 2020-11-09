//using System.Collections.Generic;
//using System.Threading.Tasks;
//using OSS.Common.BasicMos.Resp;
//using OSS.EventNode;
//using OSS.EventNode.MetaMos;
//using OSS.EventTask.Interfaces;
//using OSS.EventTask.Mos;
//using OSS.TaskFlow.Tests.TestNodes.AddNode.Tasks;
//using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;

//namespace OSS.TaskFlow.Tests.TestNodes.AddNode
//{
//    /// <summary>
//    ///   添加订单节点
//    /// </summary>
//    public class AddNode : BaseNode<AddOrderReq, Resp>
//    {
//        // 获取所有执行任务
//        private static List<IEventTask<AddOrderReq,Resp>> list;
//        protected override Task<List<IEventTask<AddOrderReq, Resp>>> GetTasks() => Task.FromResult(list);
//        public AddNode()
//        {
//            var couponTask = new CouponUseTask();
//            couponTask.TaskMeta.WithNodeMeta(NodeMeta);

//            var priceTask = new PriceComputeTask();
//            priceTask.TaskMeta.WithNodeMeta(NodeMeta);
            
//            var stockTask = new StockUseTask();
//            stockTask.TaskMeta.WithNodeMeta(NodeMeta);

//            var insertTask = new InsertOrderTask();
//            insertTask.TaskMeta.WithNodeMeta(NodeMeta);
            
//            list = new List<IEventTask<AddOrderReq, Resp>>(){
//                couponTask,priceTask,insertTask,stockTask
//            };
//        }
        
//        protected override NodeMeta GetDefaultMeta()
//        {
//            return new NodeMeta()
//            {
//                flow_id = "Order_Flow",
//                node_alias = "添加订单",
//                owner_type = OwnerType.Node,
//                Process_type = NodeProcessType.Serial,

//                node_id = "AddOrderNode"
//            };
//        }
//    }
//}
