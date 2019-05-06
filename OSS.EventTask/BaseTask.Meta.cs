using System;
using System.Threading.Tasks;
using OSS.Common.Plugs.LogPlug;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{
    public abstract partial class BaseTask<TTContext, TTRes>
    {
   
        public BaseTask(TaskMeta meta) : base(meta)
        {
        }

        #region 注册存储接口

        internal ITaskProvider m_metaProvider;

      
        #endregion

        #region 内部扩展方法

        // 领域数据需要保持独立源，且其状态会发生变化，不会保存
        internal abstract Task SaveTaskCondition(TTContext context);

        #endregion


        #region 辅助方法

        private Task TrySaveTaskContext(TTContext context)
        {
            try
            {
                // 仅task独立运行时通过这里保存
                if (OwnerType==OwnerType.Task)
                {
                    return SaveTaskCondition(context);
                }
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