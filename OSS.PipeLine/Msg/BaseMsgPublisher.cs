#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  消息发布者基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.DataFlow;
using OSS.Pipeline.Base;

namespace OSS.Pipeline
{
    /// <summary>
    ///  消息发布者基类
    /// </summary>
    /// <typeparam name="TMsg"></typeparam>
    public abstract class BaseMsgPublisher<TMsg> :   BaseThreeWayPipe<TMsg, Empty, TMsg>
    {
        // 内部异步处理入口
        private readonly IDataPublisher _pusher;

        /// <summary>
        ///  消息发布者
        /// </summary>
        /// <param name="defaultPushMsgKey">缓冲DataFlow 对应的消息Key   默认对应的flow实现是异步线程池</param>
        /// <param name="option"></param>
        protected BaseMsgPublisher(string defaultPushMsgKey, DataPublisherOption option = null) : base(defaultPushMsgKey,
            PipeType.MsgPublisher)
        {
            _pusher = RegisterPublisher(option);
        }

        #region 扩展

        /// <summary>
        ///  生成推送消息对应的key值，默认为 PipeCode（即构造函数中传入的 defaultPushMsgKey）
        ///    返回空，则 对应消息跳过发布，不做处理
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>默认返回PipeCode</returns>
        protected virtual string GeneratePushKey(TMsg msg)
        {
            return PipeCode;
        }


        /// <summary>
        ///  创建消息流
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        protected abstract IDataPublisher RegisterPublisher(DataPublisherOption option);

        #endregion

        #region 管道业务处理

        internal override async Task<TrafficResult<Empty, TMsg>> InterProcessPackage(TMsg context, string prePipeCode)
        {
            var msgKey = GeneratePushKey(context);
            if (string.IsNullOrEmpty(msgKey))
            {
                return new TrafficResult<Empty, TMsg>(SignalFlag.Green_Pass, string.Empty, string.Empty, Empty.Default,
                    context);
            }
            return (await _pusher.Publish(msgKey, context))
                ? new TrafficResult<Empty, TMsg>(SignalFlag.Green_Pass, string.Empty, string.Empty, Empty.Default, context)
                : new TrafficResult<Empty, TMsg>(SignalFlag.Red_Block, PipeCode, $"{this.GetType().Name}发布消息失败!", Empty.Default, context);
        }

        #endregion



    }

}
