namespace OSS.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    public struct WatchDataItem
    {
        /// <summary>
        ///  节点编码
        /// </summary>
        public string PipeCode { get; set; }

        /// <summary>
        ///  节点类型
        /// </summary>
        public PipeType PipeType { get; set; }

        /// <summary>
        ///  动作类型
        /// </summary>
        public WatchActionType ActionType { get; set; }

        /// <summary>
        /// 输入参数
        /// </summary>
        public object Para { get; set; }

        /// <summary>
        ///  结果
        /// </summary>
        public WatchResult Result { get; set; }
    }

    public enum WatchActionType
    {
        /// <summary>
        ///  上游管道调用
        /// </summary>
        PreCall,

        /// <summary>
        ///  执行完成
        /// </summary>
        Executed,

        /// <summary>
        ///  堵塞
        /// </summary>
        Blocked,
    }
}