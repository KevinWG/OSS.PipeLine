
using OSS.DataFlow;

namespace OSS.Pipeline.InterImpls.Msg
{
    internal class InterMsgPublisher<TMsg> : BaseMsgPublisher<TMsg>
    {
        public InterMsgPublisher(string msgDataFlowKey,DataPublisherOption option) : base(msgDataFlowKey, option)
        {
        }


        protected override IDataPublisher<TMsg> CreatePublisher(string flowKey, DataPublisherOption option)
        {
            return DataFlowFactory.CreatePublisher<TMsg>(flowKey, option);
        }
    }
}
