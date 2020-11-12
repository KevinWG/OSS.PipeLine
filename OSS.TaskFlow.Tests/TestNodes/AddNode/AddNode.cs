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
        private static IList<EventTask<AddOrderReq, Resp>> list;
 

        protected override Task<IList<EventTask<AddOrderReq, Resp>>> GetTasks(int triedTimes) => Task.FromResult(list);

        private static GroupEventTaskMeta meta = new GroupEventTaskMeta()
        {
            flow_id = "Order_Flow",
            group_alias = "添加订单",
            owner_type = OwnerType.Group,
            Process_type = GroupProcessType.Serial,

            group_id = "AddOrderNode"
        };
        public AddNode():base(meta)
        {
            var couponTask = new CouponUseTask();
            var priceTask = new PriceComputeTask();
            var stockTask = new StockUseTask();
            var insertTask = new InsertOrderTask();


            list = new List<EventTask<AddOrderReq, Resp>>(){
                couponTask,priceTask,insertTask,stockTask
            };
        }

     
    }
}
