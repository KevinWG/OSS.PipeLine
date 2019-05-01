using System;
using System.Threading.Tasks;
using OSS.Common.Plugs.LogPlug;
using OSS.EventTask.Interfaces;
using OSS.EventTask.Mos;

namespace OSS.EventTask
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
        
        #endregion


        #region 辅助方法

        private Task SaveTaskContext(TaskContext context, TaskReqData data)
        {
            try
            {
                return SaveTaskContext_Internal(context, data);
            }
            catch (Exception e)
            {
                //  防止Provider中SaveTaskContext内部使用Task实现时，级联异常死循环
                LogUtil.Error(e, "Oss.TaskFlow.Task.SaveTaskContext", "Oss.TaskFlow");
            }
            return Task.CompletedTask;
        }

        #endregion
    }
}