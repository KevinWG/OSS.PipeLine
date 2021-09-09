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
    public abstract class BaseMsgPublisher<TMsg> : BaseOneWayPipe<TMsg>
    {
        // 内部异步处理入口
        private readonly IDataPublisher<TMsg> _pusher;

        /// <summary>
        ///  消息发布者
        /// </summary>
        /// <param name="defaultPushMsgKey">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        protected BaseMsgPublisher(string defaultPushMsgKey, DataPublisherOption option = null) : base(defaultPushMsgKey,
            PipeType.MsgPublisher)
        {
            _pusher = CreatePublisher(option);
        }

        #region 扩展

        /// <summary>
        ///  生成推送消息对应的key值
        ///     默认使用构造函数传入的 defaultPushMsgKey，如果不传则为 PipeCode
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
        protected abstract IDataPublisher<TMsg> CreatePublisher(DataPublisherOption option);

        #endregion

        #region 管道业务处理

        internal override async Task<TrafficResult<TMsg, TMsg>> InterProcessPackage(TMsg context, string prePipeCode)
        {
            return (await _pusher.Publish(GeneratePushKey(context), context))
                ? new TrafficResult<TMsg, TMsg>(SignalFlag.Green_Pass, string.Empty, string.Empty, context, context)
                : new TrafficResult<TMsg, TMsg>(SignalFlag.Red_Block, PipeCode, $"{this.GetType().Name}发布消息失败!",
                    context, context);
        }

        #endregion



    }

}
