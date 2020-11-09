using System.Threading.Tasks;
using OSS.EventTask.Extention;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{
    public abstract partial class BaseTask<TTData,TTRes> : BaseMeta<TaskMeta>, IEventTask<TTData,TTRes>
        where TTData : class
        where TTRes : class,new()
    {
  
        protected BaseTask()
        {
        }

        protected BaseTask(TaskMeta meta) :base(meta)
        {
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
        protected virtual Task SaveTaskContext(TTData data, TTRes resp, RunCondition cond)
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

        private Task TrySaveTaskContext(TaskMeta taskMeta,TTData data, TaskResp<TTRes> taskResp)
        {
         
            //try
            //{
            if (taskMeta.owner_type == OwnerType.Task)
            {
                return taskResp.run_status == TaskRunStatus.RunPaused
                    ? SaveTaskContext(data, taskResp.resp, taskResp.task_cond)
                    : SaveErrorTaskContext(data, taskResp.resp, taskResp.task_cond);
            }
            //}
            //catch (Exception e)
            //{
            //    //  防止Provider中SaveTaskContext内部使用Task实现时，级联异常死循环
            //    LogUtil.Error($"Errors occurred during [Task context] saving. Detail:{e}", TaskMeta.task_id, EventTaskProvider.ModuleName);
            //}
            return Task.CompletedTask;
        }

        #endregion
    }
}