using System;
using System.Threading.Tasks;
using OSS.EventTask.Extension;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{
    public abstract class BaseEventTask<TMetaType, TTData, TResp> : BaseMeta<TMetaType>
        where TMetaType : BaseTaskMeta
        where TTData : class
        where TResp : BaseTaskResp, new()
    {
        protected BaseEventTask()
        {
        }

        protected BaseEventTask(TMetaType meta) : base(meta)
        {
        }

        #region 扩展方法

        /// <summary>
        ///  保存对应运行请求和重试相关信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="resp"></param>
        /// <returns></returns>
        protected virtual Task SaveContext(TTData data, TResp resp)
        {
            return Task.CompletedTask;
        }

        #endregion
        
        #region 任务进入入口

        public Task<TResp> Process(TTData data) => Process(data, 0);

        public async Task<TResp> Process(TTData data, int triedTimes)
        {
            var taskResp = new TResp {tried_times = triedTimes, run_status = TaskRunStatus.WaitToRun};

            await Processing(data, taskResp);

            taskResp.executed_time = DateTime.Now.ToUtcSeconds();

            // 判断是否间隔执行,生成重试信息
            if (taskResp.run_status.IsFailed()
                && taskResp.tried_times < Meta.retry_times)
            {
                taskResp.tried_times++;
                taskResp.next_time = taskResp.executed_time + Meta.retry_seconds;

                taskResp.run_status = TaskRunStatus.RunPaused;
                await SaveContext(data, taskResp);
            }

            //  最终失败，执行失败方法
            if (taskResp.run_status.IsFailed())
                await FinallyFailed(data, taskResp);

            return taskResp;
        }


        #endregion

        #region 生命周期扩展方法


        #endregion

        #region 扩展方法（实现，回退，失败）  扩展方法

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="data"></param>
        /// <param name="res"></param>
        /// <returns> 
        ///  runStatus = TaskRunStatus.RunFailed 系统会字段判断是否满足重试条件执行重试
        /// </returns>
        internal abstract Task Processing(TTData data, TResp res);
        

        /// <summary>
        /// 任务结束方法
        /// </summary>
        /// <param name="data"></param>
        /// <param name="res">请求的上下文</param>
        /// <returns></returns>
        protected virtual Task ProcessEnd(TTData data, TResp res)
        {
            return Task.CompletedTask;
        }


        /// <summary>
        ///  最终失败执行方法
        /// </summary>
        /// <param name="data"></param>
        /// <param name="res"></param>
        protected virtual Task FinallyFailed(TTData data, TResp res)
        {
            return Task.CompletedTask;
        }

        #endregion




    }
}
