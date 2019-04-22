using System.Threading.Tasks;
using OSS.EventFlow.Node;
using OSS.EventFlow.Tasks.Mos;
using OSS.EventFlow.Tests.TestOrder.Tasks;

namespace OSS.EventFlow.Tests.TestOrder.Nodes
{
    public class AddOrderNode : BaseNode<OrderInfo>
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
    public class CheckOrderNode:BaseNode<OrderInfo>
    {
        public override async Task<TaskResultMo> Call(OrderInfo para)
        {
            var context = new TaskContext<OrderInfo>(para);

            var check=new OrderCheckTask();
            await check.Process(context);
 
            var notice = new OrderNotifyTask();
            await notice.Process(context);
            return new TaskResultMo();
        }

    }


    
}
