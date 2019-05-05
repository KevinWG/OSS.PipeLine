using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.EventNode.Mos;
using OSS.EventTask;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{
    /// <inheritdoc />
    public abstract partial class BaseDomainNode<TReq, TDomain, TRes> : BaseNode<NodeContext<TReq, TDomain>, TRes>
        where TRes : ResultMo, new()
    {

        #region 扩展方法

   

        #endregion


        #region 内部扩展方法重写

        internal override async Task<TRes> GetTaskItemResult(NodeContext<TReq, TDomain> con, IBaseTask task, TaskMeta taskMeta, RunCondition taskRunCondition)
        {
            var taskContext = con.ConvertToTaskContext(taskMeta);

            if (task.InstanceType == InstanceType.Domain)
            {
                var domainTask = (BaseDomainTask<TReq, TDomain, TRes>)task;
                return await domainTask.ProcessWithRetry(taskContext,taskRunCondition);
            }

            var standTask = (BaseStandTask<TReq, TRes>)task;
            return await standTask.ProcessWithRetry(taskContext, taskRunCondition);
        }




        internal override  Task<ResultMo> ExcuteCheck(NodeContext<TReq, TDomain> context)
        {
            if (context.domain_data == null)
            {
                return Task.FromResult( new ResultMo(SysResultTypes.InnerError, ResultTypes.ObjectNull,
                    "Domain node must excute with domain_data!"));
            }
            return base.ExcuteCheck(context);
        }

        #endregion

        #region 辅助方法

  

        #endregion

    }
}
