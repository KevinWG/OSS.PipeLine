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
