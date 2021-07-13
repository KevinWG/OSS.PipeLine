#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  管道类型
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

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
        ///  消息枚举器（循环处理
        /// </summary>
        MsgEnumerator = 350,

        /// <summary>
        /// 组合管道线
        /// </summary>
        Pipeline = 1000
    }
}
