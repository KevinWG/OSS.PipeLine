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

namespace OSS.Pipeline
{
    /// <summary>
    ///  基础管道
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TOutContext"></typeparam>
    /// <typeparam name="THandlePara"></typeparam>
    public abstract class BasePipe<TInContext, THandlePara, TOutContext> : BaseHandlePipePart<TInContext, THandlePara>, IOutPipeAppender<TOutContext>
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="pipeType"></param>
        protected BasePipe(PipeType pipeType) : base(pipeType)
        {
        }

        #region 内部的业务处理 

        //internal override Task<bool> InterStart(TInContext context)
        //{
        //    throw new System.NotImplementedException($"{PipeCode} 当前的内部 InterStart 方法没有实现，无法启动");
        //}

        internal override Task<bool> InterHandling(THandlePara context)
        {
            throw new System.NotImplementedException($"{PipeCode} 当前的内部 InterHandling 方法没有实现，无法执行");
        }

        #endregion


        #region 管道连接处理

        //private 
        internal BasePipePart NextPipe { get;private set; }
        internal Task<bool> ToNextThrough(TOutContext nextInContext)
        {
            if (NextPipe != null)
            {
                if (_nextPipe!=null)
                {
                   return  _nextPipe.InterStart(nextInContext);
                }
                else
                {
                    return _nextEmptyPipe.InterStart(EmptyContext.Default);
                }
            }
            return Task.FromResult(false);
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

        #region 内部扩散方法

        internal override void InterInitialContainer(IPipeLine flowContainer)
        {
            LineContainer = flowContainer;
            if (this.Equals(flowContainer.EndPipe))
                return;

            if (NextPipe == null)
                throw new ArgumentNullException(nameof(NextPipe),
                    $"Flow({flowContainer.PipeCode})需要有明确的EndPipe，且所有的分支路径最终需到达此EndPipe");

            NextPipe.InterInitialContainer(flowContainer);
        }

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

    /// <summary>
    ///  管道执行基类
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TOutContext"></typeparam>
    public abstract class BasePipe<TInContext,TOutContext> : BasePipe<TInContext, TInContext, TOutContext>
    {
        /// <inheritdoc />
        protected BasePipe(PipeType pipeType) : base(pipeType)
        {
        }


        #region 流体启动和异步处理逻辑

        /// <summary>
        /// 启动方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<bool> Execute(TInContext context)
        {
            return InterStart(context);
        }

        #endregion

        /// <summary>
        ///  管道处理实际业务流动方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal override async Task<bool> InterStart(TInContext context)
        {
            var res = await InterHandling(context);
            if (res)
            {
                await Block(context);
            }
            return true;
        }
        
    }
}
