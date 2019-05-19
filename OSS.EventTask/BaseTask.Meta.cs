using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;

namespace OSS.EventTask
{
    public abstract partial class BaseTask<TTData, TTRes> : BaseMetaProvider<TaskMeta>, IEventTask<TTData>
        where TTData : class 
        where TTRes : ResultMo, new()
    {
        public TaskMeta TaskMeta => GetConfig();
        //internal ITaskProvider m_metaProvider;

        private const string _moduleName = "OSS.EventTask";
        //public InstanceType InstanceTaskType { get; protected set; }


        protected BaseTask() : this(null)
        {
        }
        protected BaseTask(TaskMeta meta) : base(meta)
        {
            ModuleName = _moduleName;
        }

        #region 扩展方法

        /// <summary>
        ///  保存对应运行请求和重试相关信息
        ///    【仅在 OwnerType = OwnerType.Task 时发生】
        ///     节点下的环境信息，由节点内部处理,防止节点其他耗时任务造成执行过程中发起重试操作 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="resp"></param>
        /// <param name="cond"></param>
        /// <returns></returns>
        protected virtual Task SaveTaskContext(TTData data,TTRes resp,RunCondition cond)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///  保存对应运行请求和重试相关信息
        ///    【仅在 OwnerType = OwnerType.Task 时发生】
        ///     节点下的环境信息，由节点内部处理,防止节点其他耗时任务造成执行过程中发起重试操作 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="resp"></param>
        /// <param name="cond"></param>
        /// <returns></returns>
        protected virtual Task SaveErrorTaskContext(TTData data, TTRes resp, RunCondition cond)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 辅助方法

        private Task TrySaveTaskContext(TTData data, TaskResp<TTRes> taskResp)
        {
            try
            {
                if (TaskMeta.owner_type== OwnerType.Task)
                {
                    return  taskResp.run_status==TaskRunStatus.RunPaused
                        ?SaveTaskContext(data, taskResp.resp, taskResp.task_cond)
                        :SaveErrorTaskContext(data,taskResp.resp,taskResp.task_cond);
                }
            }
            catch (Exception e)
            {
                //  防止Provider中SaveTaskContext内部使用Task实现时，级联异常死循环
                LogUtil.Error($"Errors occurred during [Task context] saving. Detail:{e}", TaskMeta.task_id, ModuleName);
            }
            return Task.CompletedTask;
        }

        #endregion
    }
}