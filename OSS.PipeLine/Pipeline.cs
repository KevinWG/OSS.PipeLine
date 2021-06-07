#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 流体基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-28
*       
*****************************************************************************/

#endregion

using System;
using System.Threading.Tasks;
using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;
using OSS.Pipeline.InterImpls.Watcher;

namespace OSS.Pipeline
{
    /// <summary>
    /// 基础流体
    /// </summary>
    /// <typeparam name="TInFlowContext"></typeparam>
    /// <typeparam name="TOutFlowContext"></typeparam>
    public class Pipeline<TInFlowContext, TOutFlowContext> : BaseStraightPipe<TInFlowContext, TOutFlowContext> , IPipeLine
    {
        private readonly BaseInPipePart<TInFlowContext>    _startPipe;
        private readonly IOutPipeAppender<TOutFlowContext> _endPipe;
        /// <summary>
        ///  开始管道
        /// </summary>
        public IPipe StartPipe => _startPipe;

        /// <summary>
        ///  结束管道
        /// </summary>
        public IPipe EndPipe => _endPipe;
        
        /// <summary>
        /// 基础流体
        /// </summary>
        public Pipeline(BaseInPipePart<TInFlowContext> startPipe, IOutPipeAppender<TOutFlowContext> endPipeAppender) : this(startPipe,endPipeAppender,null)
        {
        }

        /// <summary>
        /// 基础流体
        /// </summary>
        public Pipeline(BaseInPipePart<TInFlowContext> startPipe, IOutPipeAppender<TOutFlowContext> endPipeAppender, PipeLineOption option) : base(PipeType.Pipeline)
        {
            _startPipe = startPipe;
            _endPipe   = endPipeAppender;

            if (_startPipe == null || _endPipe == null)
            {
                throw new ArgumentNullException("未发现流体的起始截止管道！");
            }

            if (option?.Watcher!=null)
            {
                WatchProxy = new PipeWatcherProxy(option.Watcher,option.WatcherDataFlowKey,option.WatcherDataFlowOption);
            }
           
            startPipe.InterInitialContainer(this);
        }


        #region 监控

        PipeWatcherProxy IPipeLine.GetProxy()
        {
            return WatchProxy;
        }

        #endregion


        #region 管道的业务处理

        //  管道本身不再向下流动，由终结点处理
        internal override Task<bool> InterStart(TInFlowContext context)
        {
            return InterHandling(context);
        }

        /// <summary>
        ///  管道处理实际业务流动方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal override Task<bool> InterHandling(TInFlowContext context)
        {
            return _startPipe.InterStart(context);
        }

        #endregion
        
        #region 管道连接处理

        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal override void InterAppend(BaseInPipePart<TOutFlowContext> nextPipe)
        {
            _endPipe.InterAppend(nextPipe);
        }
        internal override void InterAppend(BaseInPipePart<EmptyContext> nextPipe)
        {
            _endPipe.InterAppend(nextPipe);
        }

        #endregion

        #region 路由处理
        
        private PipeRoute _route;

        /// <summary>
        ///  生成路径
        /// </summary>
        /// <returns></returns>
        public PipeRoute ToRoute()
        {
            return _route ??= InterToRoute(true);
        }

        internal override PipeRoute InterToRoute(bool isFlowSelf = false)
        {
            var pipe = new PipeRoute
            {
                pipe_code = PipeCode, pipe_type = PipeType, inter_pipe = _startPipe.InterToRoute()
            };

            if (isFlowSelf || NextPipe == null|| Equals(LineContainer.EndPipe))
            {
                return pipe;
            }

            pipe.next = NextPipe.InterToRoute();
            return pipe;
        }

     

        #endregion

    }

    /// <inheritdoc />
    public class Pipeline<TContext> : Pipeline<TContext, TContext>
    {
        /// <inheritdoc />
        public Pipeline(BaseInPipePart<TContext> startPipe, IOutPipeAppender<TContext> endPipeAppender) : base(startPipe, endPipeAppender)
        {
        }
    }
}
