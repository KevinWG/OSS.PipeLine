using System.Threading.Tasks;
using OSS.TaskFlow.FlowLine.Mos;
using OSS.TaskFlow.Tasks.Interfaces;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tasks
{
    public abstract partial class BaseTask
    {
        public InstanceType InstanceType { get; protected set; }


        #region 注册存储接口

        internal ITaskMetaProvider m_metaProvider;

        internal void RegisteProvider_Internal(ITaskMetaProvider metaPpro)
        {
            m_metaProvider = metaPpro;
        }
        
        #endregion
        

        #region 内部扩展方法

        internal abstract Task SaveTaskContext_Internal(TaskContext context, TaskReqData data);
        
        #endregion


    }
}