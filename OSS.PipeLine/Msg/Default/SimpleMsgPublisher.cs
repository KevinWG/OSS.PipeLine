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

        /// <inheritdoc />
        public SimpleMsgPublisher(Func<TMsg, string> pushKeyCreator, DataPublisherOption option = null, string pipeCode = null) : this(string.Empty, option, pipeCode)
        {
            _pushKeyGenerator = pushKeyCreator;
        }

        /// <inheritdoc />
        public SimpleMsgPublisher(string msgKey, DataPublisherOption option = null, string pipeCode = null) : base(msgKey, option, pipeCode)
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
            return DataFlowFactory.CreatePublisher(option);
        }
    }
}
