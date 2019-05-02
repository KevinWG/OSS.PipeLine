using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventNode.Mos;
using OSS.EventTask;
using OSS.EventTask.Interfaces;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{
    /// <summary>
    ///  基础领域节点
    ///   todo 获取领域信息
    /// </summary>
    public abstract partial class BaseDomainNode<TReq, TDomain, TRes> : BaseNode<NodeContext<TReq, TDomain>, TRes>
        where TRes : ResultMo, new()
    {
        
        #region 内部扩展方法重写

        internal override async Task<TRes> GetTaskItemResult(IBaseTask task, NodeContext<TReq, TDomain> con)
        {
            var taskContext = new TaskContext<TReq, TDomain>();
            if (task.InstanceType == InstanceType.Domain)
            {
                var domainTask = (BaseDomainTask<TReq, TDomain, TRes>)task;
                return await domainTask.Process(taskContext);
            }
            else
            {
                var standTask = (BaseStandTask<TReq, TRes>)task;
                return await standTask.Process(taskContext);
            }
        }

        #endregion

    }
}
