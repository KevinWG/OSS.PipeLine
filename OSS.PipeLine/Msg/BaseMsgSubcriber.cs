using System;
using System.Threading.Tasks;
using OSS.Pipeline.Mos;

namespace OSS.Pipeline.Msg
{
    /// <summary>
    ///  消息流基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseMsgSubscriber<TContext> : BasePipe<EmptyContext,TContext,TContext>
    {
        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="msgDataFlowKey">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        protected BaseMsgSubscriber(string msgDataFlowKey) : base(PipeType.BufferConnector)
        {
            msgDataFlowKey ??= string.Concat(LineContainer.PipeCode,"-", PipeCode);

             CreateSubscriber(SubscribeCaller, msgDataFlowKey);
        }

        /// <summary>
        ///  创建消息流
        /// </summary>
        /// <param name="subscribeFunc"></param>
        /// <param name="flowKey"></param>
        /// <returns></returns>
        protected abstract void CreateSubscriber(Func<TContext, Task<bool>> subscribeFunc, string flowKey);
        
        // 订阅唤起操作
        private Task<bool> SubscribeCaller(TContext data)
        {
            return ToNextThrough(data);
        }

        internal override Task<bool> InterStart(EmptyContext context)
        {
            return Task.FromResult(true);
        }
    }

}
