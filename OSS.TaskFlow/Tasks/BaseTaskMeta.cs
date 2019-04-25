using System;
using System.Threading.Tasks;
using OSS.TaskFlow.Tasks.MetaMos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tasks
{
    public abstract partial class BaseTask
    {
        public TaskMeta TaskMeta { get; set; }

        #region 重试机制设置

        /// <summary>
        ///   任务重试配置
        /// </summary>
        public TaskRetryConfig RetryConfig { get; internal set; }


        #endregion

        /// <summary>
        ///  保存
        /// </summary>
        /// <param name="context"></param>
        internal abstract Task SaveTaskContext(TaskBaseContext context);
    }


    public abstract partial class BaseTask<TReq, TRes> 
    {
        #region 重试机制设置

        /// <summary>
        ///  设置持续重试信息
        /// </summary>
        /// <param name="continueTimes"></param>
        public void SetContinueRetry(int continueTimes)
        {
            if (RetryConfig == null)
                RetryConfig = new TaskRetryConfig();

            RetryConfig.continue_times = continueTimes;
        }

        private Func<TaskContext<TReq>, Task> _contextKepper;

        /// <summary>
        ///  设置持续重试信息
        /// </summary>
        /// <param name="intTimes"></param>
        /// <param name="contextKeeper"></param>
        public void SetIntervalRetry(int intTimes, Func<TaskContext<TReq>, Task> contextKeeper)
        {
            if (RetryConfig == null)
                RetryConfig = new TaskRetryConfig();

            RetryConfig.interval_times = intTimes;
            _contextKepper = contextKeeper ?? throw new ArgumentNullException(nameof(contextKeeper),
                                 "Context Keeper will save the context info for the next time, can not be null!");
        }

        #endregion
    }
}