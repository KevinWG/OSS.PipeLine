﻿#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  默认消息订阅者实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using OSS.DataFlow;

namespace OSS.Pipeline
{
    /// <summary>
    /// 消息订阅者
    /// </summary>
    /// <typeparam name="TMsg"></typeparam>
    public class SimpleMsgSubscriber<TMsg>:BaseMsgSubscriber<TMsg>
    {
        /// <summary>
        /// 消息订阅者
        /// </summary>
        /// <param name="pipeCode"></param>
        public SimpleMsgSubscriber(string pipeCode) : base(pipeCode)
        {
        }
   

        /// <inheritdoc />
        protected override void RegisterSubscriber(string pipeDataKey, IDataSubscriber<TMsg> subscriber)
        {
            DataFlowFactory.RegisterSubscriber(pipeDataKey, subscriber);
        }
    }
}
