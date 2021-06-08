using OSS.DataFlow;

namespace OSS.Pipeline
{
    /// <summary>
    /// 消息订阅者
    /// </summary>
    /// <typeparam name="TMsg"></typeparam>
    public class MsgSubscriber<TMsg>:BaseMsgSubscriber<TMsg>
    {
        /// <summary>
        /// 消息订阅者
        /// </summary>
        /// <param name="msgDataFlowKey"></param>
        /// <param name="option"></param>
        public MsgSubscriber(string msgDataFlowKey,DataFlowOption option) : base(msgDataFlowKey, option)
        {
        }

        /// <inheritdoc />
        protected override void ReceiveSubscriber(string flowKey, IDataSubscriber<TMsg> subscriber, DataFlowOption option)
        {
            DataFlowFactory.ReceiveSubscriber(flowKey, subscriber, option);
        }
    }
}
