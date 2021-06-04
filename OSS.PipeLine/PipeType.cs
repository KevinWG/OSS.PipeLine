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
        ///  消息流
        /// </summary>
        MsgFlow = 300,
        
        /// <summary>
        ///  消息发布者
        /// </summary>
        MsgPublisher = 310,

        /// <summary>
        ///  消息订阅者
        /// </summary>
        MsgSubscriber = 320,

        /// <summary>
        ///  消息订阅者
        /// </summary>
        MsgConverter = 350,

        /// <summary>
        /// 组合管道线
        /// </summary>
        Pipeline = 1000
    }
}
