using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventTask;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;

namespace OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Tasks
{
    public class InsertOrderTask : BaseAddOrderTask
    {
        public InsertOrderTask()
        {
            TaskMeta.continue_times = 3;
            TaskMeta.task_id = "AddOrderTask";
            TaskMeta.task_alias = "添加订单！";
        }

        /// <inheritdoc />
        protected override async Task<DoResponse<ResultIdMo>> Do( AddOrderReq req)
        {
            return new DoResponse<ResultIdMo>() {resp = new ResultIdMo(), run_status = TaskRunStatus.RunCompoleted};
        }
    }

    public abstract class BaseAddOrderTask : BaseTask<AddOrderReq, ResultIdMo>
    {
        private static readonly TaskMeta taskMeta = new TaskMeta
        {
            node_id = "AddOrderNode",
            node_action = NodeResultAction.FailedOnFailed,
            owner_type = OwnerType.Flow,

            continue_times = 3,
            task_id = "AddOrderTask",
            task_alias = "添加订单！"
        };

        public BaseAddOrderTask() : base(taskMeta)
        {
        }
    }
}
