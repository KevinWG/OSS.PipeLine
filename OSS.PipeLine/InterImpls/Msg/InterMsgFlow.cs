using System;
using System.Threading.Tasks;
using OSS.Pipeline.Msg;
using OSS.Tools.DataFlow;

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
        /// <param name="msgDataFlowKey">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        public InterMsgFlow(string msgDataFlowKey) : base(msgDataFlowKey)
        {
        }

        /// <inheritdoc />
        protected override IDataPublisher<TContext> CreateFlow(Func<TContext, Task<bool>> subscribeFunc, string flowKey)
        {
            return DataFlowFactory.CreateFlow(subscribeFunc, flowKey, "OSS.Pipeline.Msg");
        }


    }


}
