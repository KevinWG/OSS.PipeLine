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
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TOutContext"></typeparam>
    public class Pipeline<TInContext, TOutContext> : BaseFourWayPipe<TInContext, TInContext, TOutContext, TOutContext>, IPipeLine<TInContext>
    {
        private readonly BaseInPipePart<TInContext>    _startPipe;
        private readonly IPipeAppender<TOutContext> _endPipe;
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
        public Pipeline(string pipeCode, BaseInPipePart<TInContext> startPipe, IPipeAppender<TOutContext> endPipeAppender) : this(pipeCode,startPipe, endPipeAppender,null)
        {
        }

        /// <summary>
        /// 基础流体
        /// </summary>
        public Pipeline(string pipeCode,BaseInPipePart<TInContext> startPipe, IPipeAppender<TOutContext> endPipeAppender, PipeLineOption option) : base(PipeType.Pipeline)
        {
            if (startPipe == null || endPipeAppender == null||string.IsNullOrEmpty(pipeCode))
            {
                throw new ArgumentNullException("未发现流体的起始截止管道！");
            }

            PipeCode   = pipeCode;
            _startPipe = startPipe;
            _endPipe   = endPipeAppender;

            if (option?.Watcher!=null)
            {
                WatchProxy = new PipeWatcherProxy(option.Watcher,option.WatcherDataFlowKey,option.WatcherDataFlowOption);
            }
           
            startPipe.InterInitialContainer(this);
        }

        #region 管道启动

        /// <inheritdoc />
        public Task<TrafficResult> Execute(TInContext context)
        {
            return InterPreCall(context);
        }

        #endregion

        #region 管道的业务处理

        //  管道本身不再向下流动，由终结点处理
        internal override Task<TrafficResult> InterPreCall(TInContext context)
        {
           return _startPipe.InterPreCall(context);
        }


        /// <inheritdoc />
        internal override Task<TrafficResult<TOutContext, TOutContext>> InterProcessPackage(TInContext context)
        {
            throw new Exception("不应该执行到此方法!");
        }

        #endregion

        #region 管道连接处理
        
        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal override void InterAppend(BaseInPipePart<TOutContext> nextPipe)
        {
            _endPipe.InterAppend(nextPipe);
        }
        internal override void InterAppend(BaseInPipePart<Empty> nextPipe)
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

        #region 监控对象处理

        PipeWatcherProxy IPipeLine.GetProxy()
        {
            return WatchProxy;
        }
        
        #endregion

    }

    /// <inheritdoc />
    public class SimplePipeline<TContext> : Pipeline<Empty, TContext>
    {
        /// <inheritdoc />
        public SimplePipeline(string pipeCode,BaseInPipePart<Empty> startPipe, IPipeAppender<TContext> endPipeAppender) : base(pipeCode,startPipe, endPipeAppender)
        {
        }
    }
}
