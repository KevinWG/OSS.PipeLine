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
    public class SimpleMsgPublisher<TMsg> : BaseMsgPublisher<TMsg>
    {
        private readonly Func<TMsg, string> _pushKeyGenerator;

        /// <summary>
        ///  消息发布者
        /// </summary>
        /// <param name="pipeCode"></param>
        /// <param name="pushKeyGenerator"></param>
        /// <param name="option"></param>
        public SimpleMsgPublisher(string pipeCode, Func<TMsg, string> pushKeyGenerator, DataPublisherOption option) : base(
            pipeCode, option)
        {
            _pushKeyGenerator = pushKeyGenerator;
        }

        /// <inheritdoc />
        public SimpleMsgPublisher(string pipeCode, DataPublisherOption option = null) : this(pipeCode, null, option)
        {
        }


        /// <inheritdoc />
        public SimpleMsgPublisher(string pipeCode, Func<TMsg, string> pushKeyCreator = null) : this(pipeCode, pushKeyCreator,
            null)
        {
        }

        /// <inheritdoc />
        protected override string GeneratePushKey(TMsg msg)
        {
            return _pushKeyGenerator!=null ? _pushKeyGenerator?.Invoke(msg) : base.GeneratePushKey(msg);
        }

        /// <inheritdoc />
        protected override IDataPublisher CreatePublisher(DataPublisherOption option)
        {
            return DataFlowFactory.CreatePublisher<TMsg>(option);
        }
    }
}
