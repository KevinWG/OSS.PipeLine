
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
        /// <param name="pipeCode"></param>
        public MsgPublisher(string pipeCode) : this(pipeCode, null)
        {
        }

        /// <summary>
        ///  消息发布者
        /// </summary>
        /// <param name="pipeCode"></param>
        /// <param name="option"></param>
        public MsgPublisher(string pipeCode, DataPublisherOption option) : base(pipeCode, option)
        {
        }


        /// <inheritdoc />
        protected override IDataPublisher<TMsg> CreatePublisher(string flowKey, DataPublisherOption option)
        {
            return DataFlowFactory.CreatePublisher<TMsg>(flowKey, option);
        }
    }
}
