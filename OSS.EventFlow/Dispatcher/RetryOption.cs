namespace OSS.EventFlow.Dispatcher
{
    /// <summary>
    ///  重试配置
    /// </summary>
    public class TaskRetryOption
    {
        /// <summary>
        ///  重试次数
        /// </summary>
        public int RetryTimes { get; set; }

        /// <summary>
        ///  重试类型
        /// </summary>
        public TaskRetryType RetryTime { get; set; }
    }


    public enum TaskRetryType
    {
        /// <summary>
        ///  连续重试
        /// </summary>
        Continue,

        /// <summary>
        ///  间隔重试
        /// </summary>
        IntervalRetry
    }
}
