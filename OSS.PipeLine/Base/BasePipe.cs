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
    public abstract class BasePipe<TInContext, THandlePara, TOutContext>
        :  BaseInPipePart<TInContext>,
        IOutPipeAppender<TOutContext>
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="pipeType"></param>
        protected BasePipe(PipeType pipeType) : base(pipeType)
        {
        }
        
        #region 管道业务扩展方法

        /// <summary>
        ///  管道堵塞
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal virtual async Task InterBlock(THandlePara context)
        {
            await Watch(PipeCode,PipeType, WatchActionType.Blocked, context); 
            await Block(context);
        }

        /// <summary>
        ///  管道堵塞
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task Block(THandlePara context)
        {
            return Task.CompletedTask;
        }


        #endregion
        
        #region 管道连接处理

        //private 
        internal BasePipePart NextPipe { get; private set; }

        internal Task<bool> ToNextThrough(TOutContext nextInContext)
        {
            if (NextPipe != null)
            {
                if (_nextPipe != null)
                {
                    return _nextPipe.InterStart(nextInContext);
                }
                else
                {
                    return _nextEmptyPipe.InterStart(EmptyContext.Default);
                }
            }

            return InterUtil.FalseTask;
        }

        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal virtual void InterAppend(BaseInPipePart<TOutContext> nextPipe)
        {
        }

        private BaseInPipePart<TOutContext> _nextPipe { get; set; }

        void IOutPipeAppender<TOutContext>.InterAppend(BaseInPipePart<TOutContext> nextPipe)
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
        internal virtual void InterAppend(BaseInPipePart<EmptyContext> nextPipe)
        {
        }

        private BaseInPipePart<EmptyContext> _nextEmptyPipe { get; set; }

        void IOutPipeAppender<TOutContext>.InterAppend(BaseInPipePart<EmptyContext> nextPipe)
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
