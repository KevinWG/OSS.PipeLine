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

using System;

namespace OSS.Pipeline
{
    /// <summary>
    ///   管道类型
    /// </summary>
   [Flags]
    public enum PipeType
    {
        /// <summary>
        ///  活动
        /// </summary>
        Activity = 1,
        /// <summary>
        ///  受控活动
        /// </summary>
        EffectActivity = 2,

        /// <summary>
        ///  被动活动
        /// </summary>
        PassiveActivity = 4,

        /// <summary>
        ///  聚合网关
        /// </summary>
        AggregateGateway = 8,
        
        /// <summary>
        ///  分支网关
        /// </summary>
        BranchGateway = 16,

        /// <summary>
        ///  消息流
        /// </summary>
        MsgFlow = 32,
        
        /// <summary>
        ///  消息发布者
        /// </summary>
        MsgPublisher = 64,

        /// <summary>
        ///  消息订阅者
        /// </summary>
        MsgSubscriber = 128,

        /// <summary>
        ///  消息订阅者
        /// </summary>
        MsgConverter = 256,

        /// <summary>
        ///  消息枚举器（循环处理
        /// </summary>
        MsgEnumerator = 512,

        /// <summary>
        /// 组合管道线
        /// </summary>
        Pipeline = 1024
    }
}
