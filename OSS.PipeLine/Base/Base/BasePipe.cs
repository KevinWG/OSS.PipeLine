#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 流体基础管道部分
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.DataFlow.Event;
using OSS.PipeLine.Base.Base.InterImpls;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.Base.Base
{
    /// <summary>
    ///  管道基类 (双入双出类型)
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="TPara"></typeparam>
    /// <typeparam name="TRes"></typeparam>
    public abstract class BasePipe<TIn, TPara,TRes, TOut>
        :  BaseInPipePart<TIn>, IPipeRetry
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="pipeCode"></param>
        /// <param name="pipeType"></param>
        protected BasePipe(string pipeCode, PipeType pipeType) : base(pipeCode, pipeType)
        {
        }

        #region 业务重试实现

        private PipeRetryEventProcessor<TPara, TrafficSignal<TRes, TOut>> _retryProcessor;
        void IPipeRetry.SetErrorRetry(FlowEventOption option)
        {
            _retryProcessor = new PipeRetryEventProcessor<TPara, TrafficSignal<TRes, TOut>>(
                    InterRetryProcessHandling, option);
        }
        private Task<TrafficSignal<TRes, TOut>> InterRetryProcessHandling(RetryEventMsg<TPara> eMsg)
        {
            return InterProcessingAndDistribute(eMsg.para);
        }

        #endregion

        #region 管道内部业务流转处理

        /// <summary>
        ///  内部管道 -- （1）执行
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        internal Task<TrafficSignal<TRes,TOut>> InterProcess(TPara req)
        {
            if (_retryProcessor!=null)
            {
                return _retryProcessor.Process(new RetryEventMsg<TPara>(req));
            }
            return InterProcessingAndDistribute(req);
        }

        /// <summary>
        ///  内部管道 -- （2）执行 -  调用监控执行 + 分发
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        internal abstract Task<TrafficSignal<TRes, TOut>> InterProcessingAndDistribute(TPara req);

        /// <summary>
        ///  内部管道 -- （3）执行 -  监控执行
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        internal async Task<TrafficSignal<TRes, TOut>> InterWatchProcessing(TPara req)
        {
            var trafficRes = await InterProcessing(req);
            await Watch(PipeCode, PipeType, WatchActionType.Executed, req, trafficRes.ToWatchResult()).ConfigureAwait(false);

            return trafficRes;
        }

        /// <summary>
        ///     具体执行实现
        /// </summary>
        internal abstract Task<TrafficSignal<TRes, TOut>> InterProcessing(TPara req);

        /// <summary>
        ///  管道堵塞  --  (4) 执行 - 阻塞实现
        /// </summary>
        /// <param name="req"></param>
        /// <param name="tRes"></param>
        /// <returns></returns>
        internal async Task InterWatchBlock(TPara req, TrafficSignal<TRes, TOut> tRes)
        {
            await Watch(PipeCode, PipeType, WatchActionType.Blocked, req, tRes.ToWatchResult())
                .ConfigureAwait(false);
            await Block(req, tRes);
        }

        #endregion

        #region 管道外部扩展
        
        /// <summary>
        ///  管道堵塞（堵塞可能来自本管道，也可能是通知下游管道返回堵塞
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tRes"></param>
        /// <returns></returns>
        protected virtual Task Block(TPara req, TrafficSignal<TRes, TOut> tRes)
        {
            return Task.CompletedTask;
        }
        
        #endregion
    }

}
