using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Tasks.Mos;
using OSS.TaskFlow.Tests.TestOrder.Tasks;

namespace OSS.TaskFlow.Tests.TestOrder.Nodes
{
    public class AddOrderNode : TaskFlow.Node.BaseNode<OrderInfo>
    {
        public override async Task<ResultMo> Call(OrderInfo para)
        {
            var context = new TaskContext<OrderInfo>();

            var addTask = new AddOrderTask();
            await addTask.Process(context);

            var notice = new OrderNotifyTask();
            await notice.Process(context);
            return new ResultMo();
        }
    }
    public class CheckOrderNode: TaskFlow.Node.BaseNode<OrderCheckReq>
    {
        public override async Task<ResultMo> Call(OrderCheckReq para)
        {
            var context = new TaskContext<OrderCheckReq>(para);

            var check=new OrderCheckTask();
            await check.Process(context);
 
            //var notice = new OrderNotifyTask();
            //await notice.Process(context);
            return new ResultMo();
        }

    }


    
}
