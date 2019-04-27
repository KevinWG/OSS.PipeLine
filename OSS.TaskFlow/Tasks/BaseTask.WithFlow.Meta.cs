using System;
using System.Threading.Tasks;
using OSS.TaskFlow.FlowLine.Mos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tasks
{
    
    public abstract partial class BaseTask<TReq, TFlowData, TRes> 
    {
        public InstanceType InstanceType { get; }

        public BaseTask()
        {
            InstanceType = InstanceType.WithFlow;
        }
        

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

        private Func<TaskContext,TaskReqData<TReq,TFlowData>, Task> _contextKepper;

        /// <summary>
        ///  设置持续重试信息
        /// </summary>
        /// <param name="intTimes"></param>
        /// <param name="contextKeeper"></param>
        public void SetIntervalRetry(int intTimes, Func<TaskContext, TaskReqData<TReq, TFlowData>, Task> contextKeeper)
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