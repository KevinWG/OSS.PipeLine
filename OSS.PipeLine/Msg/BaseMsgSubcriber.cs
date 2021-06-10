using System;
using System.Threading.Tasks;
using OSS.DataFlow;
using OSS.Pipeline.Base;

namespace OSS.Pipeline
{
    /// <summary>
    ///  消息流基类
    /// </summary>
    /// <typeparam name="TMsg"></typeparam>
    public abstract class BaseMsgSubscriber<TMsg> : BaseFuncPipe<TMsg, TMsg>, IDataSubscriber<TMsg>
    {
        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="pipeCode">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        protected BaseMsgSubscriber(string pipeCode) : this(pipeCode, null)
        {
        }

        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="pipeCode">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        /// <param name="option">数据流配置信息</param>
        protected BaseMsgSubscriber(string pipeCode, DataFlowOption option) : base(PipeType.MsgSubscriber)
        {
            if (string.IsNullOrEmpty(pipeCode))
            {
                throw new ArgumentNullException(nameof(pipeCode), "消息类型PipeCode不能为空!");
            }

            ReceiveSubscriber(pipeCode, this, option);
        }

        /// <summary>
        ///  创建消息流
        /// </summary>
        /// <param name="subscribeFunc"></param>
        /// <param name="option"></param>
        /// <param name="flowKey"></param>
        /// <returns></returns>
        protected abstract void ReceiveSubscriber(string flowKey, IDataSubscriber<TMsg> subscribeFunc,
            DataFlowOption option);

        /// <summary>
        ///  订阅消息的动作实现
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> Subscribe(TMsg data)
        {
            return (await ToNextThrough(data)) == TrafficSignal.Green_Pass;
        }
    }

}
