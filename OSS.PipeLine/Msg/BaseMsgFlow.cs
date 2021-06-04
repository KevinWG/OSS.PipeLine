using System;
using System.Threading.Tasks;
using OSS.Tools.DataFlow;

namespace OSS.Pipeline.Msg
{
    /// <summary>
    ///  消息流基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseMsgFlow<TContext> : BasePipe<TContext, TContext>
    {
        // 内部异步处理入口
        private readonly IDataPublisher<TContext> _pusher;

        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="msgDataFlowKey">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        protected BaseMsgFlow(string msgDataFlowKey) : base(PipeType.BufferConnector)
        {
            msgDataFlowKey ??= PipeCode;

            _pusher = CreateFlow(SubscribeCaller, msgDataFlowKey);
        }

        /// <summary>
        /// 异步缓冲连接器
        /// 缓冲DataFlow 对应的Key为当前PipeCode
        /// </summary>
        protected BaseMsgFlow() : this(null)
        {
        }

        /// <summary>
        ///  创建消息流
        /// </summary>
        /// <param name="subscribeFunc"></param>
        /// <param name="flowKey"></param>
        /// <returns></returns>
        protected abstract IDataPublisher<TContext> CreateFlow(Func<TContext, Task<bool>> subscribeFunc, string flowKey);

        // 订阅唤起操作
        private Task<bool> SubscribeCaller(TContext data)
        {
            return ToNextThrough(data);
        }

        internal override Task<bool> InterHandling(TContext context)
        {
            return _pusher.Publish(context);
        }
    }

}
