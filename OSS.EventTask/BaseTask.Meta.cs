using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{
    public abstract partial class BaseTask<TTReq, TTRes, TReq> : BaseMetaProvider<TaskMeta>, IBaseTask<TTReq>
        where TTReq : ExcuteReq<TReq>
        where TTRes : ResultMo, new()
    {
        public TaskMeta TaskMeta => GetConfig();
        internal ITaskProvider m_metaProvider;

        private const string _moduleName = "OSS.EventTask";
        public InstanceType InstanceTaskType { get; protected set; }

        protected BaseTask(TaskMeta meta) : base(meta)
        {
            ModuleName = _moduleName;
        }

        #region 扩展方法



        /// <summary>
        ///  保存环境相关信息【主要有两种情况，一种是 Pause，一种是 Failed】
        ///    【仅在 OwnerType = OwnerType.Task 时发生】
        ///     节点下的环境信息，由节点内部处理,防止节点其他耗时任务造成执行过程中发起重试操作 
        ///     领域数据需要保持独立源，且其状态会受其他并行节点发生变化，这里不会保存
        /// </summary>
        /// <param name="req"></param>
        /// <param name="taskResp"></param>
        /// <returns></returns>
        protected virtual Task SaveTaskCondition(ExcuteReq<TReq> req, TaskResponse<TTRes> taskResp)
        {
            return Task.CompletedTask;
        }

        #endregion
        
        #region 辅助方法

        private Task TrySaveTaskContext(TTReq req, TaskResponse<TTRes> taskResp)
        {
            try
            {
                if (TaskMeta.owner_type== OwnerType.Task)
                {
                    return SaveTaskCondition(req, taskResp);
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