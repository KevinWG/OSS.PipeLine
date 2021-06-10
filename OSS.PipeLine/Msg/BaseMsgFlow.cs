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
    public abstract class BaseMsgFlow<TMsg> : BaseStraightPipe<TMsg, TMsg>,IDataSubscriber<TMsg>
    {
        // 内部异步处理入口
        private readonly IDataPublisher<TMsg> _pusher;
        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="pipeCode"> 作为缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        protected BaseMsgFlow(string pipeCode) : this(pipeCode, null)
        {
        }

        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="pipeCode">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        protected BaseMsgFlow(string pipeCode, DataFlowOption option) : base(PipeType.MsgFlow)
        {
            if (string.IsNullOrEmpty(pipeCode))
            {
                throw new ArgumentNullException(nameof(pipeCode), "消息类型PipeCode不能为空!");
            }
            _pusher = CreateFlow(pipeCode, this, option);
        }
        
        /// <summary>
        ///  创建消息流
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="option"></param>
        /// <param name="flowKey"></param>
        /// <returns></returns>
        protected abstract IDataPublisher<TMsg> CreateFlow(string flowKey,IDataSubscriber<TMsg> subscriber, DataFlowOption option);

        internal override async Task<TrafficSignal> InterHandling(TMsg context)
        {
            return (await _pusher.Publish(context)) ? TrafficSignal.Green_Pass : TrafficSignal.Red_Block;
        }

        /// <summary>
        ///  订阅唤起操作
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> Subscribe(TMsg data)
        {
            return (await ToNextThrough(data))==TrafficSignal.Green_Pass;
        }
    }

}
