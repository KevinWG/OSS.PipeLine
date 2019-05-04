using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.EventTask.Interfaces;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{
    public abstract partial class BaseTask<TTContext,TTRes> where TTContext : TaskContext
        where TTRes : ResultMo, new()
    {
        public InstanceType InstanceType { get; protected set; }
        public RunType RunType { get; protected set; }

        protected BaseTask()
        {
            RunType = RunType.None;
        }


        #region 注册存储接口

        internal ITaskProvider m_metaProvider;

        internal void RegisteProvider_Internal(ITaskProvider taskPro)
        {
            m_metaProvider = taskPro;
        }

        #endregion

        #region 内部扩展方法

        protected virtual Task SaveTaskContext(TTContext context, RunCondition runCondition)
        {
            return Task.CompletedTask;
        }

        #endregion


        #region 辅助方法

        private Task TrySaveTaskContext(TTContext context,RunCondition runCondition)
        {
            try
            {
                return SaveTaskContext(context, runCondition);
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