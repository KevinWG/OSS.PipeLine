
using OSS.DataFlow;

namespace OSS.Pipeline
{
    /// <summary>
    ///  消息发布者
    /// </summary>
    /// <typeparam name="TMsg"></typeparam>
    public class MsgPublisher<TMsg> : BaseMsgPublisher<TMsg>
    {
        /// <summary>
        ///  消息发布者
        /// </summary>
        /// <param name="msgDataFlowKey"></param>
        /// <param name="option"></param>
        public MsgPublisher(string msgDataFlowKey,DataPublisherOption option) : base(msgDataFlowKey, option)
        {
        }


        protected override IDataPublisher<TMsg> CreatePublisher(string flowKey, DataPublisherOption option)
        {
            return DataFlowFactory.CreatePublisher<TMsg>(flowKey, option);
        }
    }
}
