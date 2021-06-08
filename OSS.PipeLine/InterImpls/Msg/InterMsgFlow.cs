using OSS.DataFlow;

namespace OSS.Pipeline.InterImpls.Msg
{
    /// <summary>
    /// 异步消息延缓连接器
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    internal class InterMsgFlow<TContext> : BaseMsgFlow<TContext>
    {
        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="pipeCode">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        public InterMsgFlow(string pipeCode, DataFlowOption option) : base(pipeCode, option)
        {
        }

        protected override IDataPublisher<TContext> CreateFlow(string flowKey, IDataSubscriber<TContext> subscriber, DataFlowOption option)
        {
            return DataFlowFactory.CreateFlow(flowKey, subscriber, option);
        }
    }


}
