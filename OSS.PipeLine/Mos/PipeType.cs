namespace OSS.EventFlow.Mos
{
    /// <summary>
    ///   管道类型
    /// </summary>
    public enum PipeType
    {
        /// <summary>
        ///  直接活动
        /// </summary>
        Activity = 100,

        /// <summary>
        ///   独立Action活动
        /// </summary>
        ActionActivity = 110,


        /// <summary>
        ///  聚合网关
        /// </summary>
        AggregateGateway =200,

        /// <summary>
        ///  分支网关
        /// </summary>
        BranchGateway=210,
        

        /// <summary>
        ///  连接通道
        /// </summary>
        Connector = 300,


        /// <summary>
        /// 组合管道
        /// </summary>
        Flow = 1000
    }
}
