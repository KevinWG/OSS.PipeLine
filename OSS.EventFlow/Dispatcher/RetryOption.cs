namespace OSS.EventFlow.Dispatcher
{
    /// <summary>
    ///  重试配置
    /// </summary>
    public class TaskRetryConfig
    {
        /// <summary>
        ///  直接重试次数
        /// </summary>
        public int DirectTimes { get; set; }

        /// <summary>
        ///  间隔重试次数
        /// </summary>
        public int IntervalTimes { get; set; }

        ///// <summary>
        /////  重试类型
        ///// </summary>
        //public TaskRetryType RetryType { get; set; }
    }


    //public enum TaskRetryType
    //{
    //    /// <summary>
    //    ///  不需要重试
    //    /// </summary>
    //    None,
        
    //    /// <summary>
    //    ///  连续重试
    //    /// </summary>
    //    Direct,

    //    /// <summary>
    //    ///  间隔重试
    //    /// </summary>
    //    Interval,

    //    /// <summary>
    //    ///    先直接重试，再执行间隔重试
    //    /// </summary>
    //    DirectThenInterval
    //}
}
