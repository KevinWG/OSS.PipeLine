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
using OSS.Pipeline.Base.Base;
using System;
using System.Threading.Tasks;
using OSS.Pipeline.InterImpls.Watcher;
using OSS.Pipeline.InterImpls;

namespace OSS.Pipeline.Base
{
    /// <summary>
    ///  管道基类 (双入双出类型)
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TOutContext"></typeparam>
    /// <typeparam name="THandlePara"></typeparam>
    /// <typeparam name="THandleResult"></typeparam>
    public abstract class BaseFourWayPipe<TInContext, THandlePara,THandleResult, TOutContext>
        : BasePipe<TInContext, THandlePara, THandleResult, TOutContext>,
        IPipeAppender<TOutContext>
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="pipeType"></param>
        protected BaseFourWayPipe(PipeType pipeType) : base(pipeType)
        {
        }

        #region 管道内部业务流转处理

        /// <inheritdoc />
        internal override async Task<TrafficResult<THandleResult, TOutContext>> InterProcessHandling(THandlePara context, string prePipeCode)
        {
            var trafficRes = await InterProcessPackage(context,prePipeCode);
            await Watch(PipeCode, PipeType, WatchActionType.Executed, context, trafficRes.ToWatchResult());

            if (trafficRes.signal == SignalFlag.Green_Pass)
            {
                var nextTrafficRes = await ToNextThrough(trafficRes.next_paras);
                return new TrafficResult<THandleResult, TOutContext>(
                    nextTrafficRes.signal, nextTrafficRes.blocked_pipe_code, nextTrafficRes.msg
                    , trafficRes.result, trafficRes.next_paras
                );
            }

            if (trafficRes.signal == SignalFlag.Red_Block)
            {
                await InterBlock(context, trafficRes);
            }

            return trafficRes;
        }

        #endregion
        
        #region 管道连接处理
        
        internal BasePipePart NextPipe { get; set; }

        internal virtual Task<TrafficResult> ToNextThrough(TOutContext nextInContext)
        {
            if (NextPipe != null)
            {
                if (_nextPipe != null)
                {
                    return _nextPipe.InterPreCall(nextInContext,PipeCode);
                }
                else
                {
                    return _nextEmptyPipe.InterPreCall(Empty.Default,PipeCode);
                }
            }
            // 说明已经是最后一个管道
            return  InterUtil.GreenTrafficResultTask; 
        }


        private BaseInPipePart<TOutContext> _nextPipe { get; set; }
        void IPipeAppender<TOutContext>.InterAppend(BaseInPipePart<TOutContext> nextPipe)
        {
            InterAppend(nextPipe);
            nextPipe.InterAppendTo(this);
        }

        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal virtual void InterAppend(BaseInPipePart<TOutContext> nextPipe)
        {
            if (NextPipe != null)
            {
                throw new ArgumentException("当前节点已经关联下游节点！");
            }
            NextPipe = _nextPipe = nextPipe;
        }


        private BaseInPipePart<Empty> _nextEmptyPipe { get; set; }
        void IPipeAppender<TOutContext>.InterAppend(BaseInPipePart<Empty> nextPipe)
        {
            InterAppend(nextPipe);
            nextPipe.InterAppendTo(this);
        }
        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道 ( 接收空上下文
        /// </summary>
        /// <param name="nextPipe"></param>
        internal virtual void InterAppend(BaseInPipePart<Empty> nextPipe)
        {
            if (NextPipe != null)
            {
                throw new ArgumentException("当前节点已经关联下游节点！");
            }
            NextPipe = _nextEmptyPipe = nextPipe;
        }


        #endregion

        #region 管道初始化

        /// <inheritdoc />
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
