using OSS.Pipeline.Msg;
using OSS.Tools.DataFlow;

namespace OSS.Pipeline.InterImpls.Msg
{
    internal class InterMsgSubscriber<TMsg>:BaseMsgSubscriber<TMsg>
    {
        public InterMsgSubscriber(string msgDataFlowKey) : base(msgDataFlowKey)
        {
        }
        
        protected override void ReceiveSubscriber(IDataSubscriber<TMsg> subscribeFunc, string flowKey)
        { 
            DataFlowFactory.ReceiveSubscriber(subscribeFunc, flowKey, "OSS.Pipeline.Msg");
        }

    }
}
