#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  默认消息发布者实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion
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
