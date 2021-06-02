using System.Threading.Tasks;
using OSS.Pipeline.Mos;
using OSS.Tools.DataFlow;

namespace OSS.Pipeline.Connector
{
    /// <summary>
    /// 异步消息延缓连接器
    /// </summary>
    /// <typeparam name="InContext"></typeparam>
    /// <typeparam name="OutContext"></typeparam>
    public abstract class BaseBufferConnector<InContext, OutContext> : BaseMsgConvertor<InContext, OutContext>
    {
        // 内部异步处理入口
        private readonly IDataPublisher<InContext> _pusher;

        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="bufferDataFlowKey">缓冲DataFlow 对应的Key</param>
        protected BaseBufferConnector(string bufferDataFlowKey) : base(PipeType.BufferConnector)
        {
            bufferDataFlowKey ??= PipeCode;

            _pusher = DataFlowFactory.CreateFlow<InContext>(SubscribeCaller, bufferDataFlowKey, "OSS.Pipeline.Connector");
        }

        /// <summary>
        /// 异步缓冲连接器
        /// 缓冲DataFlow 对应的Key为当前PipeCode
        /// </summary>
        protected BaseBufferConnector() : this(null)
        {
        }

        // 订阅唤起操作
        private Task<bool> SubscribeCaller(InContext data)
        {
            var outContext = Convert(data);
            return ToNextThrough(outContext);
        }

        internal override Task<bool> InterHandling(InContext context)
        {
            return _pusher.Publish(context);
        }
    }


    /// <summary>
    /// 异步消息延缓连接器
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseBufferConnector<TContext> : BaseBufferConnector<TContext, TContext>
    {
        /// <inheritdoc />
        protected override TContext Convert(TContext inContextData)
        {
            return inContextData;
        }
    }
}
