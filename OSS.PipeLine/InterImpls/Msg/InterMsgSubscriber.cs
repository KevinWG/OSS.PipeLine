using OSS.DataFlow;

namespace OSS.Pipeline.InterImpls.Msg
{
    internal class InterMsgSubscriber<TMsg>:BaseMsgSubscriber<TMsg>
    {
        public InterMsgSubscriber(string msgDataFlowKey,DataFlowOption option) : base(msgDataFlowKey, option)
        {
        }
        protected override void ReceiveSubscriber(string flowKey, IDataSubscriber<TMsg> subscriber, DataFlowOption option)
        {
            DataFlowFactory.ReceiveSubscriber(flowKey, subscriber, option);
        }
    }
}
