using OSS.Pipeline.Msg;
using OSS.Tools.DataFlow;

namespace OSS.Pipeline.InterImpls.Msg
{
    internal class InterMsgPublisher<TMsg> : BaseMsgPublisher<TMsg>
    {
        public InterMsgPublisher(string msgDataFlowKey) : base(msgDataFlowKey)
        {
        }

        protected override IDataPublisher<TMsg> CreatePublisher(string flowKey)
        {
            return DataFlowFactory.CreatePublisher<TMsg>(flowKey, "OSS.Pipeline.Msg");
        }
    }
}
