using System;
using System.Threading.Tasks;
using OSS.TaskFlow.FlowLine.Mos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tasks
{
    
    public abstract partial class BaseFlowTask<TReq, TFlowData, TRes> 
    {

        protected BaseFlowTask()
        {
            InstanceType = InstanceType.WithFlow;
        }
        

        #region 重试机制设置

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