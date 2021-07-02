#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  默认消息订阅者基类
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
    public abstract class BaseMsgSubscriber<TMsg> : BaseThreeWayPassivePipe<TMsg, TMsg, TMsg>, IDataSubscriber<TMsg>
    {
        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="pipeCode">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        protected BaseMsgSubscriber(string pipeCode = null) : this(pipeCode, null)
        {
        }

        /// <summary>
        ///  异步缓冲连接器
        /// </summary>
        /// <param name="pipeCode">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        /// <param name="option">数据流配置信息</param>
        protected BaseMsgSubscriber(string pipeCode, DataFlowOption option) : base(pipeCode, PipeType.MsgSubscriber)
        {
            if (string.IsNullOrEmpty(pipeCode))
            {
                throw new ArgumentNullException(nameof(pipeCode), "消息类型PipeCode不能为空!");
            }
            ReceiveSubscriber(pipeCode, this, option);
        }

        /// <summary>
        ///  创建消息流
        /// </summary>
        /// <param name="subscribePassive"></param>
        /// <param name="option"></param>
        /// <param name="pipeDataKey"></param>
        /// <returns></returns>
        protected abstract void ReceiveSubscriber(string pipeDataKey, IDataSubscriber<TMsg> subscribePassive,
            DataFlowOption option);

        /// <summary>
        ///  订阅消息的动作实现
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> Subscribe(TMsg data)
        {
            return (await InterProcess(data,string.Empty)).signal==SignalFlag.Green_Pass;
        }

        internal override Task<TrafficResult<TMsg, TMsg>> InterProcessPackage(TMsg context, string prePipeCode)
        {
            return Task.FromResult(new TrafficResult<TMsg, TMsg>(TrafficSignal.GreenSignal, string.Empty, context,context));
        }
    }

}
