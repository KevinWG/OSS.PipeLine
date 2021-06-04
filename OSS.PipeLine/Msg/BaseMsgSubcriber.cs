using System;
using System.Threading.Tasks;
using OSS.Tools.DataFlow;

namespace OSS.Pipeline.Msg
{
    /// <summary>
    ///  消息流基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseMsgSubscriber<TContext> : BasePipe<EmptyContext,TContext,TContext>, IDataSubscriber<TContext>
    {
        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="msgDataFlowKey">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        protected BaseMsgSubscriber(string msgDataFlowKey) : base(PipeType.BufferConnector)
        {
            msgDataFlowKey ??= string.Concat(LineContainer.PipeCode,"-", PipeCode);

             ReceiveSubscriber(this, msgDataFlowKey);
        }

        /// <summary>
        ///  创建消息流
        /// </summary>
        /// <param name="subscribeFunc"></param>
        /// <param name="flowKey"></param>
        /// <returns></returns>
        protected abstract void ReceiveSubscriber(IDataSubscriber<TContext> subscribeFunc, string flowKey);
        
        internal override Task<bool> InterStart(EmptyContext context)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        ///  订阅消息的动作实现
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task<bool> Subscribe(TContext data)
        {
            return ToNextThrough(data);
        }
    }

}
