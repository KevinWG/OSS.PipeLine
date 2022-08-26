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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;
using OSS.Pipeline.InterImpls.Watcher;

namespace OSS.Pipeline
{
    /// <summary>
    /// 基础流体
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public class Pipeline<TIn, TOut> :
        BaseFourWayPipe<TIn, TIn, TOut, TOut>, IPipeLine<TIn, TOut>
    {
        #region 首尾节点定义

        private readonly BaseInPipePart<TIn> _startPipe;
        private readonly IPipeAppender<TOut> _endPipe;

        /// <summary>
        ///  开始管道
        /// </summary>
        public IPipeMeta StartPipe => _startPipe;

        /// <summary>
        ///  结束管道
        /// </summary>
        public IPipeMeta EndPipe => _endPipe;

        #endregion
        
        #region 构造函数

        /// <summary>
        /// 基础流体
        /// </summary>
        public Pipeline(string pipeCode, BaseInPipePart<TIn> startPipe,
            IPipeAppender<TOut> endPipeAppender) : this(pipeCode, startPipe, endPipeAppender, null)
        {
        }

        /// <summary>
        /// 基础流体
        /// </summary>
        public Pipeline(string pipeCode, BaseInPipePart<TIn> startPipe,
            IPipeAppender<TOut> endPipeAppender, PipeLineOption option) : base(pipeCode, PipeType.Pipeline)
        {
            if (startPipe == null || endPipeAppender == null || string.IsNullOrEmpty(pipeCode))
            {
                throw new ArgumentNullException("未发现流体的起始截止管道和管道编码！");
            }

            PipeCode   = pipeCode;
            _startPipe = startPipe;
            _endPipe   = endPipeAppender;

            InitialPipes();

            if (option?.Watcher != null)
            {
                WatchProxy = new PipeWatcherProxy(option.Watcher, option.WatcherDataFlowKey,
                    option.WatcherDataFlowOption);
            }

            startPipe.InterInitialContainer(this);
        }

        /// <summary>
        /// 初始化节点关系
        /// （构造函数中会调用
        /// </summary>
        protected virtual void InitialPipes()
        {
            
        }


        #endregion

        #region 管道的业务处理


        #region 管道业务启动

        /// <inheritdoc />
        public Task Execute(TIn context)
        {
            return InterPreCall(context);
        }

        #endregion

        /// <inheritdoc />
        internal override Task<TrafficSignal> InterPreCall(TIn context)
        {
            return _startPipe.InterPreCall(context);
        }

        /// <inheritdoc />
        internal override Task<TrafficSignal<TOut, TOut>> InterProcessing(TIn context)
        {
            throw new Exception("不应该执行到此方法!");
        }

        #endregion

        #region 管道连接重写处理

        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal override void InterAppend(IPipeInPart<TOut> nextPipe)
        {
            base.InterAppend(nextPipe);     // 保证路由初始化，本身next节点不会被执行
            _endPipe.InterAppend(nextPipe); // 保证业务执行
        }

        internal override void InterAppend(IPipeInPart<Empty> nextPipe)
        {
            base.InterAppend(nextPipe);     // 保证路由初始化，本身next节点不会被执行
            _endPipe.InterAppend(nextPipe); // 保证业务执行
        }

        #endregion

        #region Pipeline 路由 管理

        private Dictionary<string ,PipeLink> _linkDics;
        Dictionary<string, PipeLink> IPipeLine.GetLinkDics()
        {
            if (_linkDics==null)
            {
                _linkDics = new Dictionary<string, PipeLink>();
            }
            return _linkDics;
        }

        /// <summary>
        ///  生成路径
        /// </summary>
        /// <returns></returns>
        public List<PipeLink> ToRoute()
        {
            if (_linkDics==null)
            {
                InterFormatLink(string.Empty,true);
            }
            return _linkDics.Select(x=>x.Value).ToList();
        }

        internal override void InterFormatLink(string prePipeCode, bool isSelf = false)
        {
            if (isSelf)
            {
                _startPipe.InterFormatLink(string.Empty);
            }
            else
            {
                base.InterFormatLink(prePipeCode);
            }
        }

        #endregion

        #region 管道初始化

        /// <inheritdoc />
        internal override void InterInitialContainer(IPipeLine flowContainer)
        {
            base.InterInitialContainer(flowContainer);
            _startPipe.InterInitialContainer(this);
        }

        #endregion

        #region 管道 内部监控代理器

        /// <inheritdoc />
        PipeWatcherProxy IPipeLine.GetWatchProxy() => WatchProxy;

        #endregion

    }
}
