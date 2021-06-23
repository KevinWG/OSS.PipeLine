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

using OSS.Pipeline.Interface;
using System;
using System.Threading.Tasks;
using OSS.Pipeline.InterImpls.Watcher;

namespace OSS.Pipeline.Base
{
    /// <summary>
    ///  管道基类
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TOutContext"></typeparam>
    /// <typeparam name="THandlePara"></typeparam>
    /// <typeparam name="THandleResult"></typeparam>
    public abstract class BasePipe<TInContext, THandlePara,THandleResult, TOutContext>
        :  BaseInPipePart<TInContext>,
        IPipeAppender<TOutContext>
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="pipeType"></param>
        protected BasePipe(PipeType pipeType) : base(pipeType)
        {
        }
        
        #region 管道内部业务流转处理
        
        /// <summary>
        ///  内部管道 -- （2）执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal async Task<TrafficResult<THandleResult,TOutContext>> InterExecute(THandlePara context)
        {
            var res = await InterHandling(context);

            if (res.signal == SignalFlag.Red_Block)
            {
                await InterBlock(context, res);
            }

            return res;
        }

        /// <summary>
        ///  内部管道 -- （3）执行 - 控制流转
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal virtual async Task<TrafficResult<THandleResult, TOutContext>> InterHandling(THandlePara context)
        {
            var trafficRes = await InterHandlePack(context);
            await Watch(PipeCode, PipeType, WatchActionType.Executed, context, trafficRes.ToWatchResult());

            if (trafficRes.signal == SignalFlag.Green_Pass)
            {
                var nextTrafficRes = await ToNextThrough(trafficRes.next_paras);
                return new TrafficResult<THandleResult, TOutContext>(
                    nextTrafficRes.signal, nextTrafficRes.blocked_pipe_code, nextTrafficRes.msg
                    , trafficRes.result, trafficRes.next_paras
                );
            }

            return trafficRes;
        }

        /// <summary>
        ///  内部管道 -- （4）执行 - 组装业务处理结果
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal abstract Task<TrafficResult<THandleResult, TOutContext>> InterHandlePack(THandlePara context);

        #endregion

        #region 管道阻塞扩展


        /// <summary>
        ///  管道堵塞
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tRes"></param>
        /// <returns></returns>
        internal virtual async Task InterBlock(THandlePara context, TrafficResult<THandleResult, TOutContext> tRes)
        {
            await Watch(PipeCode, PipeType, WatchActionType.Blocked, context, tRes.ToWatchResult());
            await Block(context, tRes);
        }

        /// <summary>
        ///  管道堵塞
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tRes"></param>
        /// <returns></returns>
        protected virtual Task Block(THandlePara context, TrafficResult<THandleResult, TOutContext> tRes)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 管道连接处理

        //private 
        internal BasePipePart NextPipe { get; private set; }

        internal Task<TrafficResult> ToNextThrough(TOutContext nextInContext)
        {
            if (NextPipe != null)
            {
                if (_nextPipe != null)
                {
                    return _nextPipe.InterStart(nextInContext);
                }
                else
                {
                    return _nextEmptyPipe.InterStart(Empty.Default);
                }
            }
            return Task.FromResult(new TrafficResult(SignalFlag.Red_Block,PipeCode,"未发现下一步管道信息！"));
        }

        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal virtual void InterAppend(BaseInPipePart<TOutContext> nextPipe)
        {
        }

        private BaseInPipePart<TOutContext> _nextPipe { get; set; }

        void IPipeAppender<TOutContext>.InterAppend(BaseInPipePart<TOutContext> nextPipe)
        {
            if (NextPipe != null)
            {
                throw new ArgumentException("当前节点已经关联下游节点！");
            }

            NextPipe = _nextPipe = nextPipe;
            InterAppend(nextPipe);
        }



        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道 ( 接收空上下文
        /// </summary>
        /// <param name="nextPipe"></param>
        internal virtual void InterAppend(BaseInPipePart<Empty> nextPipe)
        {
        }

        private BaseInPipePart<Empty> _nextEmptyPipe { get; set; }

        void IPipeAppender<TOutContext>.InterAppend(BaseInPipePart<Empty> nextPipe)
        {
            if (NextPipe != null)
            {
                throw new ArgumentException("当前节点已经关联下游节点！");
            }

            NextPipe = _nextEmptyPipe = nextPipe;
            InterAppend(nextPipe);
        }

        #endregion

        #region 管道初始化

        internal override void InterInitialContainer(IPipeLine flowContainer)
        {
            LineContainer = flowContainer;
            WatchProxy=flowContainer.GetProxy();

            if (this.Equals(flowContainer.EndPipe))
                return;

            if (NextPipe == null)
                throw new ArgumentNullException(nameof(NextPipe),
                    $"Flow({flowContainer.PipeCode})需要有明确的EndPipe，且所有的分支路径最终需到达此EndPipe");

            NextPipe.InterInitialContainer(flowContainer);
        }

        #endregion

        #region 管道路由


        internal override PipeRoute InterToRoute(bool isFlowSelf = false)
        {
            var pipe = new PipeRoute()
            {
                pipe_code = PipeCode,
                pipe_type = PipeType
            };

            if (NextPipe == null || Equals(LineContainer.EndPipe))
                return pipe;

            pipe.next = NextPipe.InterToRoute();
            return pipe;
        }


        #endregion

    }
    
}
