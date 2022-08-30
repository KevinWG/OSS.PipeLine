#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  默认消息流体实现
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
    /// 消息流
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    internal class SimpleMsgFlow<TContext> : BaseMsgFlow<TContext>
    {
        /// <inheritdoc />
        public SimpleMsgFlow(string msgKey, string pipeCode = null) : base(msgKey, pipeCode)
        {
        }

        /// <inheritdoc />
        public SimpleMsgFlow(string msgKey, DataFlowOption option, string pipeCode = null) : base(msgKey, option, pipeCode)
        {
        }

        protected override IDataPublisher CreateFlow(string pipeDataKey, IDataSubscriber<TContext> subscriber, DataFlowOption option)
        {
            return DataFlowFactory.RegisterFlow(pipeDataKey, subscriber, option);
        }

    }


}
