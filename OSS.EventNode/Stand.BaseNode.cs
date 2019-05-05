using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.EventNode.Mos;
using OSS.EventTask;
using OSS.EventTask.Interfaces;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{
    /// <summary>
    ///  基础工作节点
    /// </summary>
    public abstract partial class BaseStandNode<TReq, TRes> : BaseNode<NodeContext<TReq>, TRes>
        where TRes : ResultMo, new()
    {
        #region 内部扩展方法重写

        internal override async Task<TRes> GetTaskItemResult(IBaseTask task, NodeContext<TReq> con)
        {
            if (task.InstanceType == InstanceType.Domain)
            {
                throw new ResultException(SysResultTypes.InnerError, ResultTypes.InnerError,
                    "StandNode can't use DomainTask!");
            }
            var taskContext = new TaskContext<TReq>(); // todo 完善
            var standTask = (BaseStandTask<TReq, TRes>) task;
            return await standTask.Process(taskContext);
        }

        #endregion
    }
}
