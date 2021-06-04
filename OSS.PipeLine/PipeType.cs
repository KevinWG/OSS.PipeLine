namespace OSS.Pipeline
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
        ///  直接活动
        /// </summary>
        EffectActivity = 110,

        /// <summary>
        ///   独立Action活动
        /// </summary>
        FuncActivity = 120,

        /// <summary>
        ///   独立Action活动
        /// </summary>
        FuncEffectActivity = 130,


        /// <summary>
        ///  聚合网关
        /// </summary>
        AggregateGateway = 200,

        /// <summary>
        ///  分支网关
        /// </summary>
        BranchGateway = 210,


        /// <summary>
        ///  连接通道
        /// </summary>
        Connector = 300,

        /// <summary>
        ///  连接通道
        /// </summary>
        BufferConnector = 310,


        /// <summary>
        /// 组合管道线
        /// </summary>
        Pipeline = 1000
    }
}
