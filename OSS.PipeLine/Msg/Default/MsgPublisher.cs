#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  默认消息发布者实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System;
using OSS.DataFlow;

namespace OSS.Pipeline
{
    /// <summary>
    ///  消息发布者
    /// </summary>
    /// <typeparam name="TMsg"></typeparam>
    public class MsgPublisher<TMsg> : BaseMsgPublisher<TMsg>
    {
        private readonly Func<TMsg, string> _pushKeySelector;

        /// <summary>
        ///  消息发布者
        /// </summary>
        /// <param name="pipeCode"></param>
        /// <param name="option"></param>
        public MsgPublisher(string pipeCode, Func<TMsg, string> pushKeyCreator, DataPublisherOption option) : base(
            pipeCode, option)
        {
            _pushKeySelector = pushKeyCreator;
        }

        /// <inheritdoc />
        public MsgPublisher(string pipeCode, DataPublisherOption option = null) : this(pipeCode, null, option)
        {
        }


        /// <inheritdoc />
        public MsgPublisher(string pipeCode, Func<TMsg, string> pushKeyCreator = null) : this(pipeCode, pushKeyCreator,
            null)
        {
        }

        /// <inheritdoc />
        protected override string GeneratePushKey(TMsg msg)
        {
            return _pushKeySelector?.Invoke(msg) ?? PipeCode;
        }

        /// <inheritdoc />
        protected override IDataPublisher<TMsg> CreatePublisher(DataPublisherOption option)
        {
            return DataFlowFactory.CreatePublisher<TMsg>(option);
        }
    }
}
