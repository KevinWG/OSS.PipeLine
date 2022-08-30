#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  消息发布者基类
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
    ///  消息发布者基类
    /// </summary>
    /// <typeparam name="TMsg"></typeparam>
    public abstract class BaseMsgPublisher<TMsg> :   BaseThreeWayPipe<TMsg, Empty, TMsg>
    {
        // 内部异步处理入口
        private readonly IDataPublisher _pusher;
        private readonly string         _msgKey;

        /// <summary>
        ///  消息发布者
        /// </summary>
        /// <param name="msgKey">缓冲DataFlow 对应的消息Key   默认对应的flow实现是异步线程池</param>
        /// <param name="option"></param>
        /// <param name="pipeCode"></param>
        protected BaseMsgPublisher(string msgKey, DataPublisherOption option = null, string pipeCode = null) : base(pipeCode, PipeType.MsgPublisher)
        {
            if (string.IsNullOrEmpty(msgKey))
            {
                throw new ArgumentNullException(nameof(msgKey), "消息类型 msgKey 不能为空!");
            }

            _msgKey = msgKey; 
            _pusher = CreatePublisher(option);
        }

        #region 扩展

        /// <summary>
        ///  生成推送消息对应的key值，默认为 PipeCode（即构造函数中传入的 defaultPushMsgKey）
        ///    返回空，则 对应消息跳过发布，不做处理
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>默认返回PipeCode</returns>
        protected virtual string GeneratePushKey(TMsg msg)
        {
            return _msgKey;
        }

        /// <summary>
        ///  创建消息流
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        protected abstract IDataPublisher CreatePublisher(DataPublisherOption option);

        #endregion

        #region 管道业务处理

        internal override async Task<TrafficSignal<Empty, TMsg>> InterProcessing(TMsg context)
        {
            var msgKey = GeneratePushKey(context);
            if (string.IsNullOrEmpty(msgKey))
            {
                return new TrafficSignal<Empty, TMsg>(Empty.Default, context);
            }
            return (await _pusher.Publish(msgKey, context))
                ? new TrafficSignal<Empty, TMsg>( Empty.Default, context)
                : new TrafficSignal<Empty, TMsg>(SignalFlag.Red_Block, Empty.Default, context, $"{this.GetType().Name}发布消息失败!");
        }

        #endregion
    }

}
