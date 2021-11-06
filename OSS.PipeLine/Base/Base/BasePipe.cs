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

namespace OSS.Pipeline.Base.Base
{
    /// <summary>
    ///  管道基类 (双入双出类型)
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TOutContext"></typeparam>
    /// <typeparam name="THandlePara"></typeparam>
    /// <typeparam name="THandleResult"></typeparam>
    public abstract class BasePipe<TInContext, THandlePara,THandleResult, TOutContext>
        :  BaseInPipePart<TInContext>
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

        private PipeRetryEventProcessor<THandlePara, TrafficResult<THandleResult, TOutContext>> _retryProcessor;
        internal void SetErrorRetry(FlowEventOption option)
        {
            _retryProcessor = new PipeRetryEventProcessor<THandlePara, TrafficResult<THandleResult, TOutContext>>(
                    InterRetryProcessHandling, option);
        }
        private Task<TrafficResult<THandleResult, TOutContext>> InterRetryProcessHandling(RetryEventMsg<THandlePara> eMsg)
        {
            return InterProcessHandling(eMsg.para, eMsg.pre_pipe_code);
        }

        #endregion

        #region 管道内部业务流转处理

        /// <summary>
        ///  内部管道 -- （1）执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prePipeCode">上游节点PipeCode</param>
        /// <returns></returns>
        internal Task<TrafficResult<THandleResult,TOutContext>> InterProcess(THandlePara context,string prePipeCode)
        {
            if (_retryProcessor!=null)
            {
                return _retryProcessor.Process(new RetryEventMsg<THandlePara>(context, prePipeCode));
            }
            return InterProcessHandling(context,prePipeCode);
        }

        /// <summary>
        ///  内部管道 -- （2）执行 - 控制流转，阻塞处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prePipeCode">上游节点PipeCode</param>
        /// <returns></returns>
        internal abstract Task<TrafficResult<THandleResult, TOutContext>> InterProcessHandling(THandlePara context, string prePipeCode);

        /// <summary>
        ///  内部管道 -- （3）执行 - 组装业务处理结果
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prePipeCode">上游节点PipeCode</param>
        /// <returns></returns>
        internal async Task<TrafficResult<THandleResult, TOutContext>> InterWatchProcessPackage(THandlePara context,
            string prePipeCode)
        {
            var trafficRes = await InterProcessPackage(context, prePipeCode);
            await Watch(PipeCode, PipeType, WatchActionType.Executed, context, trafficRes.ToWatchResult()).ConfigureAwait(false);

            return trafficRes;
        }

        internal abstract Task<TrafficResult<THandleResult, TOutContext>> InterProcessPackage(THandlePara context, string prePipeCode);

        /// <summary>
        ///  管道堵塞  --  (4) 执行 - 阻塞实现
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tRes"></param>
        /// <returns></returns>
        internal async Task InterWatchBlock(THandlePara context, TrafficResult<THandleResult, TOutContext> tRes)
        {
            await Watch(PipeCode, PipeType, WatchActionType.Blocked, context, tRes.ToWatchResult())
                .ConfigureAwait(false);
            await Block(context, tRes);
        }

        #endregion

        #region 管道外部扩展
        
        /// <summary>
        ///  管道堵塞（堵塞可能来自本管道，也可能是通知下游管道返回堵塞
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tRes"></param>
        /// <returns></returns>
        protected virtual Task Block(THandlePara context, TrafficResult<THandleResult, TOutContext> tRes)
        {
            return Task.CompletedTask;
        }
        
        #endregion
    }

}
