using System.Threading.Tasks;
using OSS.EventFlow.Tasks.Mos;
using OSS.TaskFlow.Tests.TestOrder.Tasks;

namespace OSS.TaskFlow.Tests.TestOrder.Nodes
{
    public class AddOrderNode : TaskFlow.Node.BaseNode<OrderInfo>
    {
        public override async Task<TaskResultMo> Call(OrderInfo para)
        {
            var context = new TaskContext<OrderInfo>();

            var addTask = new AddOrderTask();
            await addTask.Process(context);

            var notice = new OrderNotifyTask();
            await notice.Process(context);
            return new TaskResultMo();
        }
    }
    public class CheckOrderNode: TaskFlow.Node.BaseNode<OrderCheckReq>
    {
        public override async Task<TaskResultMo> Call(OrderCheckReq para)
        {
            var context = new TaskContext<OrderCheckReq>(para);

            var check=new OrderCheckTask();
            await check.Process(context);
 
            //var notice = new OrderNotifyTask();
            //await notice.Process(context);
            return new TaskResultMo();
        }

    }


    
}
