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
    internal class MsgFlow<TContext> : BaseMsgFlow<TContext>
    {
        /// <summary>
        ///  消息流
        /// </summary>
        /// <param name="pipeCode">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        public MsgFlow(string pipeCode) : this(pipeCode, null)
        {
        }

        /// <summary>
        ///  消息流
        /// </summary>
        /// <param name="pipeCode">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        public MsgFlow(string pipeCode, DataFlowOption option) : base(pipeCode, option)
        {
        }

        protected override IDataPublisher<TContext> CreateFlow(string flowKey, IDataSubscriber<TContext> subscriber, DataFlowOption option)
        {
            return DataFlowFactory.CreateFlow(flowKey, subscriber, option);
        }

    }


}
