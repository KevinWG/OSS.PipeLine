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
    ///  消息订阅器
    /// </summary>
    /// <typeparam name="TMsg"></typeparam>
    public abstract class BaseMsgSubscriber<TMsg> : BaseThreeWayPassivePipe<TMsg, Empty, TMsg>, IDataSubscriber<TMsg>
    {
        /// <summary>
        ///  消息订阅器
        /// </summary>
        /// <param name="pipeDataKey">缓冲DataFlow 对应的Key   默认对应的flow是异步线程池</param>
        protected BaseMsgSubscriber(string pipeDataKey) : base(pipeDataKey, PipeType.MsgSubscriber)
        {
            if (string.IsNullOrEmpty(pipeDataKey))
            {
                throw new ArgumentNullException(nameof(pipeDataKey), "消息类型PipeCode不能为空!");
            }
            RegisterSubscriber(pipeDataKey, this);
        }

        /// <summary>
        ///  接收消息订阅器
        /// </summary>
        /// <param name="subscribeHandler">消息订阅器（引用句柄）</param>
        /// <param name="option">订阅处理选项</param>
        /// <param name="pipeDataKey">订阅消息key</param>
        /// <returns></returns>
        protected abstract void RegisterSubscriber(string pipeDataKey, IDataSubscriber<TMsg> subscribeHandler);

        /// <summary>
        ///  订阅消息的动作实现
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> Subscribe(TMsg data)
        {
            return (await InterProcess(data)).signal==SignalFlag.Green_Pass;
        }

        internal override Task<TrafficSignal<Empty, TMsg>> InterProcessing(TMsg context)
        {
            return Task.FromResult(new TrafficSignal<Empty, TMsg>(Empty.Default, context));
        }
    }

}
