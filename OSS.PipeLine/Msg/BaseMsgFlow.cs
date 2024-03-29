﻿#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  消息流体基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System;
using System.Threading.Tasks;
using OSS.DataFlow;
using OSS.Pipeline.Base;

namespace OSS.Pipeline
{
    /// <summary>
    ///  消息流基类
    /// </summary>
    /// <typeparam name="TMsg"></typeparam>
    public abstract class BaseMsgFlow<TMsg> : BaseThreeWayPipe<TMsg, Empty, TMsg>,IDataSubscriber<TMsg>
    {
        // 内部异步处理入口
        private readonly IDataPublisher _pusher;
        private readonly string         _msgKey;

        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="msgKey"> 作为缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        /// <param name="pipeCode"></param>
        protected BaseMsgFlow(string msgKey,string pipeCode = null) : this(msgKey, null, pipeCode)
        {
        }


        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="pipeCode"></param>
        /// <param name="msgKey">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        protected BaseMsgFlow(string msgKey, DataFlowOption option, string pipeCode = null) : base(pipeCode, PipeType.MsgFlow)
        {
            if (string.IsNullOrEmpty(msgKey))
            {
                throw new ArgumentNullException(nameof(msgKey), "消息类型 msgKey 不能为空!");
            }

            _msgKey = msgKey;
            _pusher = CreateFlow(msgKey, this, option);
        }

        /// <summary>
        ///  创建消息流
        /// </summary>
        /// <param name="msgKey"></param>
        /// <param name="subscriber"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        protected abstract IDataPublisher CreateFlow(string msgKey, IDataSubscriber<TMsg> subscriber, DataFlowOption option);

        #region 流体内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficSignal> InterPreCall(TMsg context)
        {
            var pushRes = await _pusher.Publish(_msgKey, context);
            return pushRes
                ? TrafficSignal.GreenSignal
                : new TrafficSignal(SignalFlag.Red_Block, $"({this.GetType().Name})推送消息失败!");
        }

        /// <inheritdoc />
        internal override Task<TrafficSignal<Empty, TMsg>> InterProcessing(TMsg context)
        {
            return Task.FromResult(new TrafficSignal<Empty, TMsg>(SignalFlag.Green_Pass, Empty.Default, context));
        }
        
        #endregion

        /// <summary>
        ///  订阅唤起操作
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> Subscribe(TMsg data)
        {
            return (await InterProcess(data)).signal==SignalFlag.Green_Pass;
        }
    }

}
