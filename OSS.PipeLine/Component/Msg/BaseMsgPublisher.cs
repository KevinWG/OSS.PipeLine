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

using System;
using System.Threading.Tasks;
using OSS.DataFlow;
using OSS.Pipeline.Base;

namespace OSS.Pipeline
{
    /// <summary>
    ///  消息流基类
    /// </summary>
    /// <typeparam name="TMsg"></typeparam>
    public abstract class BaseMsgPublisher<TMsg> : BaseOneWayPipe<TMsg>
    {
        // 内部异步处理入口
        private readonly IDataPublisher<TMsg> _pusher;

        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="pipeCode">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        protected BaseMsgPublisher(string pipeCode = null) : this(pipeCode, null)
        {
        }

        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="pipeCode">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        protected BaseMsgPublisher(string pipeCode, DataPublisherOption option) : base(pipeCode, PipeType.MsgPublisher)
        {
            if (string.IsNullOrEmpty(pipeCode))
            {
                throw new ArgumentNullException(nameof(pipeCode), "消息类型PipeCode不能为空!");
            }
            _pusher = CreatePublisher(pipeCode, option);
        }

        #region 扩展

        //protected virtual string 


        /// <summary>
        ///  创建消息流
        /// </summary>
        /// <param name="pipeDataKey"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        protected abstract IDataPublisher<TMsg> CreatePublisher(string pipeDataKey, DataPublisherOption option);

        #endregion

        #region 管道业务处理

        internal override async Task<TrafficResult<TMsg, TMsg>> InterProcessPackage(TMsg context, string prePipeCode)
        {
            return (await _pusher.Publish(context))
                ? new TrafficResult<TMsg, TMsg>(SignalFlag.Green_Pass,string.Empty,string.Empty,context,context)
                : new TrafficResult<TMsg, TMsg>(SignalFlag.Red_Block,PipeCode, $"{this.GetType().Name}发布消息失败!", context, context);
        }
      
        #endregion

    

    }

}
