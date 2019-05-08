using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.EventNode.Mos;
using OSS.EventTask;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{
    /// <summary>
    ///  基础工作节点
    /// </summary>
    public abstract partial class BaseStandNode<TReq, TRes> : BaseNode<NodeResponse<TRes>, TRes>
        where TRes : ResultMo, new()
    {
        #region 内部扩展方法重写

        internal override async Task<TaskResponse<TRes>> GetTaskItemResult(NodeResponse<TReq, TRes> con, IBaseTask task, TaskMeta taskMeta, RunCondition taskRunCondition)
        {
            if (task.InstanceType == InstanceType.Domain)
            {
                throw new ResultException(SysResultTypes.ApplicationError, ResultTypes.InnerError,
                    "StandNode can't use Domain Task!");
            }

            var taskContext = con.ConvertToTaskContext(taskMeta);
            taskContext.task_condition =new RunCondition();

            var standTask = (BaseStandTask<TReq, TRes>) task;
            return await standTask.Run(taskContext);
        }

        #endregion
        
        #region 辅助方法

        #endregion
    }
}
