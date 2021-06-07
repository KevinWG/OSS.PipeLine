using System.Threading.Tasks;
using OSS.Pipeline.Interface;
using OSS.DataFlow;
using OSS.Pipeline.Base;
using OSS.Pipeline.InterImpls.Watcher;

namespace OSS.Pipeline
{
    /// <summary>
    ///  消息流基类
    /// </summary>
    /// <typeparam name="TMsg"></typeparam>
    public abstract class BaseMsgPublisher<TMsg> : BaseInterceptPipe<TMsg>
    {
        // 内部异步处理入口
        private readonly IDataPublisher<TMsg> _pusher;

        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="msgDataFlowKey">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        protected BaseMsgPublisher(string msgDataFlowKey) : this(msgDataFlowKey, null)
        {
        }
        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="msgDataFlowKey">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        protected BaseMsgPublisher(string msgDataFlowKey, DataPublisherOption option) : base(PipeType.MsgPublisher)
        {
            msgDataFlowKey ??= string.Concat(LineContainer.PipeCode, "-", PipeCode);

            _pusher = CreatePublisher(msgDataFlowKey, option);
        }


        #region 扩展

        /// <summary>
        ///  创建消息流
        /// </summary>
        /// <param name="flowKey"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        protected abstract IDataPublisher<TMsg> CreatePublisher(string flowKey, DataPublisherOption option);

        #endregion

        #region 管道业务处理

        internal override Task<bool> InterIntercept(TMsg context)
        {
            return _pusher.Publish(context);
        }

        #endregion
        
        #region 管道初始化

        internal override void InterInitialContainer(IPipeLine flowContainer)
        {
            LineContainer = flowContainer;
            WatchProxy    = flowContainer.GetProxy();
        }

        #endregion

        #region 管道路由
        //  消息发布节点本身是一个独立的结束节点
        internal override PipeRoute InterToRoute(bool isFlowSelf = false)
        {
            var pipe = new PipeRoute()
            {
                pipe_code = PipeCode,
                pipe_type = PipeType
            };
            return pipe;
        }
        
        #endregion

    }

}
