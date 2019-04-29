using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.TaskFlow.Tasks.Interfaces;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tasks
{
    public abstract partial class BaseTask
    {
        public InstanceType InstanceType { get; protected set; }


        #region 注册存储接口

        internal ITaskProvider m_metaProvider;

        internal void RegisteProvider_Internal(ITaskProvider metaPpro)
        {
            m_metaProvider = metaPpro;
        }
        
        #endregion
        
        #region 内部扩展方法

        internal abstract Task SaveTaskContext_Internal(TaskContext context, TaskReqData data);

        internal virtual Task<ResultIdMo> GenerateRunId(TaskContext context)
        {
            return Task.FromResult(new ResultIdMo(SysResultTypes.ConfigError, ResultTypes.ObjectStateError, "Task with data cann't generate run_id by itself!"));
        }

        #endregion


    }
}