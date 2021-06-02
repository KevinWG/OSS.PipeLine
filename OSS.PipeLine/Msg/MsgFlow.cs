using System.Threading.Tasks;
using OSS.Pipeline.Mos;
using OSS.Tools.DataFlow;

namespace OSS.Pipeline.Msg
{
    /// <summary>
    /// 异步消息延缓连接器
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class MsgFlow<TContext> : BasePipe<TContext, TContext>
    {
        // 内部异步处理入口
        private readonly IDataPublisher<TContext> _pusher;

        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="msgDataFlowKey">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        public MsgFlow(string msgDataFlowKey) : base(PipeType.BufferConnector)
        {
            msgDataFlowKey ??= PipeCode;

            _pusher = DataFlowFactory.CreateFlow<TContext>(SubscribeCaller, msgDataFlowKey, "OSS.Pipeline.Msg");
        }

        /// <summary>
        /// 异步缓冲连接器
        /// 缓冲DataFlow 对应的Key为当前PipeCode
        /// </summary>
        public MsgFlow() : this(null)
        {
        }

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
