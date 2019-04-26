using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Node;
using OSS.TaskFlow.Node.MetaMos;
using OSS.TaskFlow.Tasks;
using OSS.TaskFlow.Tasks.MetaMos;
using OSS.TaskFlow.Tasks.Mos;
using OSS.TaskFlow.Tests.TestOrder.Tasks;

namespace OSS.TaskFlow.Tests.TestOrder.Nodes
{
    public class AddOrderNode : BaseNode<OrderInfo>
    {

        public AddOrderNode()
        {
            NodeMeta=new NodeMeta();

            NodeMeta.node_type = NodeType.Parallel;
            NodeMeta.node_key = "AddOrderNode";
        }

        public override Task<ResultListMo<TaskMeta>> GetTaskMetas(ExcuteReq req)
        {
            var addOrderTaskMeta=new TaskMeta();
            addOrderTaskMeta.task_key = "AddOrder";
            addOrderTaskMeta.task_name = "添加订单";
            addOrderTaskMeta.run_type = RunType.FailedBreak;
            addOrderTaskMeta.node_key = NodeMeta.node_key;
            addOrderTaskMeta.status = TaskMetaStatus.Enable;
            
            var notifyTaskMeta = new TaskMeta();
            notifyTaskMeta.task_key = "Notify";
            notifyTaskMeta.task_name = "添加订单通知";
            notifyTaskMeta.run_type = RunType.FailedBreak;
            notifyTaskMeta.node_key = NodeMeta.node_key;
            notifyTaskMeta.status = TaskMetaStatus.Enable;

            var list = new List<TaskMeta>();
            list.Add(addOrderTaskMeta);
            list.Add(notifyTaskMeta);

            return Task.FromResult(new ResultListMo<TaskMeta>(list));
        }

        public override BaseTask GetTaskByMeta(TaskMeta meta)
        {
            if (meta.task_key== "AddOrder")
            {
                 return new AddOrderTask();
            }

            if (meta.task_key == "AddOrder")
            {
                return new OrderNotifyTask();
            }

            return null;
        }
    }
    public class CheckOrderNode: BaseNode<OrderCheckReq>
    {
        public override Task<ResultListMo<TaskMeta>> GetTaskMetas(ExcuteReq req)
        {
            var addOrderTaskMeta = new TaskMeta();
            addOrderTaskMeta.task_key = "CheckOrder";
            addOrderTaskMeta.task_name = "校验订单";
            addOrderTaskMeta.run_type = RunType.FailedBreak;
            addOrderTaskMeta.node_key = NodeMeta.node_key;
            addOrderTaskMeta.status = TaskMetaStatus.Enable;


            var notifyTaskMeta = new TaskMeta();
            notifyTaskMeta.task_key = "Notify";
            notifyTaskMeta.task_name = "添加订单通知";
            notifyTaskMeta.run_type = RunType.FailedBreak;
            notifyTaskMeta.node_key = NodeMeta.node_key;
            notifyTaskMeta.status = TaskMetaStatus.Enable;

            var list = new List<TaskMeta>();
            list.Add(addOrderTaskMeta);
            list.Add(notifyTaskMeta);

            return Task.FromResult(new ResultListMo<TaskMeta>(list));
        }

        public override BaseTask GetTaskByMeta(TaskMeta meta)
        {
            if (meta.task_key == "CheckOrder")
            {
                return new OrderCheckTask();
            }

            if (meta.task_key == "AddOrder")
            {
                return new OrderNotifyTask();
            }

            return null;
        }
    }


    
}
