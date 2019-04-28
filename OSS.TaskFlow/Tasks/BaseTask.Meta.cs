using System.Threading.Tasks;
using OSS.TaskFlow.FlowLine.Mos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tasks
{
    public abstract partial class BaseTask
    {
        public InstanceType InstanceType { get; protected set; }
        
        #region 重试机制设置

        /// <summary>
        ///   任务重试配置
        /// </summary>
        public TaskRetryConfig RetryConfig { get; internal set; }

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

        #endregion

        /// <summary>
        ///  保存
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        internal abstract Task SaveTaskContext(TaskContext context, TaskReqData data);
    }
}