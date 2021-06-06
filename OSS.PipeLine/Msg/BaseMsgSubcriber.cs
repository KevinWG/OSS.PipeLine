using System.Threading.Tasks;
using OSS.DataFlow;

namespace OSS.Pipeline
{
    /// <summary>
    ///  消息流基类
    /// </summary>
    /// <typeparam name="TMsg"></typeparam>
    public abstract class BaseMsgSubscriber<TMsg> : BasePipe<EmptyContext,TMsg,TMsg>, IDataSubscriber<TMsg>
    {

        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="msgDataFlowKey">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        protected BaseMsgSubscriber(string msgDataFlowKey) :this(msgDataFlowKey,null)
        {
        }

        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="msgDataFlowKey">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        /// <param name="option">数据流配置信息</param>
        protected BaseMsgSubscriber(string msgDataFlowKey, DataFlowOption option) : base(PipeType.MsgSubscriber)
        {
            msgDataFlowKey ??= string.Concat(LineContainer.PipeCode, "-", PipeCode);
            
            ReceiveSubscriber(msgDataFlowKey,this, option);
        }

        /// <summary>
        ///  创建消息流
        /// </summary>
        /// <param name="subscribeFunc"></param>
        /// <param name="option"></param>
        /// <param name="flowKey"></param>
        /// <returns></returns>
        protected abstract void ReceiveSubscriber(string flowKey, IDataSubscriber<TMsg> subscribeFunc,  DataFlowOption option);
        
        internal override Task<bool> InterStart(EmptyContext context)
        {
            return InterUtil.TrueTask;
        }

        /// <summary>
        ///  订阅消息的动作实现
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task<bool> Subscribe(TMsg data)
        {
            return ToNextThrough(data);
        }
    }

}
