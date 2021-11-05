﻿#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

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
    public class Pipeline<TInContext, TOutContext> : 
        BaseFourWayPipe<TInContext, TInContext, TOutContext, TOutContext>, IPipeLine<TInContext, TOutContext>
    {
        private readonly BaseInPipePart<TInContext>    _startPipe;
        private readonly IPipeAppender<TOutContext> _endPipe;

        /// <summary>
        ///  开始管道
        /// </summary>
        public IPipeMeta StartPipe => _startPipe;

        /// <summary>
        ///  结束管道
        /// </summary>
        public IPipeMeta EndPipe => _endPipe;
        
        /// <summary>
        /// 基础流体
        /// </summary>
        public Pipeline(string pipeCode, BaseInPipePart<TInContext> startPipe, IPipeAppender<TOutContext> endPipeAppender) : this(pipeCode,startPipe, endPipeAppender,null)
        {
        }

        /// <summary>
        /// 基础流体
        /// </summary>
        public Pipeline(string pipeCode,BaseInPipePart<TInContext> startPipe, IPipeAppender<TOutContext> endPipeAppender, PipeLineOption option) : base(pipeCode,PipeType.Pipeline)
        {
            if (startPipe == null || endPipeAppender == null||string.IsNullOrEmpty(pipeCode))
            {
                throw new ArgumentNullException("未发现流体的起始截止管道和管道编码！");
            }

            PipeCode   = pipeCode;
            _startPipe = startPipe;
            _endPipe   = endPipeAppender;

            InitialPipes();

            if (option?.Watcher!=null)
            {
                WatchProxy = new PipeWatcherProxy(option.Watcher,option.WatcherDataFlowKey,option.WatcherDataFlowOption);
            }
           
            startPipe.InterInitialContainer(this);
        }

        /// <summary>
        ///  初始化节点连接
        /// </summary>
        protected virtual void InitialPipes()
        {
        }

        #region 管道启动

        /// <inheritdoc />
        public Task Execute(TInContext context)
        {
            return InterPreCall(context,string.Empty);
        }

        #endregion

        #region 管道的业务处理

        /// <inheritdoc />
        internal override Task<TrafficResult> InterPreCall(TInContext context, string prePipeCode)
        {
           return _startPipe.InterPreCall(context,prePipeCode);
        }

        /// <inheritdoc />
        internal override Task<TrafficResult<TOutContext, TOutContext>> InterProcessPackage(TInContext context, string prePipeCode)
        {
            throw new Exception("不应该执行到此方法!");
        }

        #endregion

        #region 管道连接处理
        
        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal override void InterAppend(IPipeInPart<TOutContext> nextPipe)
        {
            base.InterAppend(nextPipe);
            _endPipe.InterAppend(nextPipe);
        }
        internal override void InterAppend(IPipeInPart<Empty> nextPipe)
        {
            base.InterAppend(nextPipe);
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
                pipe_code = PipeCode, pipe_type = PipeType, flow_sub_pipe = _startPipe.InterToRoute(false)
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
}
