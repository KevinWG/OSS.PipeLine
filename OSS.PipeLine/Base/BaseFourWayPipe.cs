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
using OSS.Pipeline.InterImpls;

namespace OSS.Pipeline.Base
{

    /// <summary>
    ///  管道基类 (双入双出类型)
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="TPara"></typeparam>
    /// <typeparam name="TRes"></typeparam>
    public abstract class BaseFourWayPipe<TIn, TPara,TRes, TOut>
        : BasePipe<TIn, TPara, TRes, TOut>
          , IPipe<TIn, TOut>
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="pipeCode"></param>
        /// <param name="pipeType"></param>
        protected BaseFourWayPipe(string pipeCode, PipeType pipeType) : base(pipeCode, pipeType)
        {
        }

        #region 管道内部业务流转处理

        /// <inheritdoc />
        internal override async Task<TrafficSignal<TRes, TOut>> InterProcessingAndDistribute(TPara req)
        {
            var trafficRes = await InterWatchProcessing(req);

            switch (trafficRes.signal)
            {
                case SignalFlag.Green_Pass:
                {
                    //  下一级执行
                    await ToNextThrough(trafficRes.output);
                    break;
                }
                case SignalFlag.Red_Block:
                    await InterWatchBlock(req, trafficRes);
                    break;
            }

            return trafficRes;
        }

        #endregion
        
        #region 管道连接处理
        
        internal IPipeInitiator NextPipe { get; set; }
        internal virtual Task<TrafficSignal> ToNextThrough(TOut nextInContext)
        {
            if (NextPipe != null)
            {
                return _nextPipe != null 
                    ? _nextPipe.InterWatchPreCall(nextInContext) 
                    : _nextEmptyPipe.InterWatchPreCall(Empty.Default);
            }
            // 说明已经是最后一个管道
            return  InterUtil.GreenTrafficSignalTask; 
        }
        

        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal virtual void InterAppend(IPipeInPart<TOut> nextPipe)
        {
            if (NextPipe != null)
            {
                throw new ArgumentException($"当前节点{PipeCode}已经关联下游节点！");
            }
            NextPipe = _nextPipe = nextPipe;
        }
        private IPipeInPart<TOut> _nextPipe { get; set; }
        void IPipeAppender<TOut>.InterAppend(IPipeInPart<TOut> nextPipe)
        {
            InterAppend(nextPipe);
        }


        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道 ( 接收空上下文
        /// </summary>
        /// <param name="nextPipe"></param>
        internal virtual void InterAppend(IPipeInPart<Empty> nextPipe)
        {
            if (NextPipe != null)
            {
                throw new ArgumentException("当前节点已经关联下游节点！");
            }
            NextPipe = _nextEmptyPipe = nextPipe;
        }
        private IPipeInPart<Empty> _nextEmptyPipe { get; set; }
        void IPipeAppender.InterAppend(IPipeInPart<Empty> nextPipe)
        {
            InterAppend(nextPipe);
        }

        #endregion

        #region 管道初始化

        /// <inheritdoc />
        internal override void InterInitialContainer(IPipeLine flowContainer)
        {
            LineContainer = flowContainer;
            WatchProxy=flowContainer.GetWatchProxy();

            if (this.Equals(flowContainer.EndPipe))
                return;

            if (NextPipe == null)
                throw new ArgumentNullException(nameof(NextPipe),
                    $"Flow({flowContainer.PipeCode})需要有明确的EndPipe，且所有的分支路径最终需到达此EndPipe");

            NextPipe.InterInitialContainer(flowContainer);
        }

        #endregion

        #region 管道路由


        internal override void InterFormatLink(string prePipeCode, bool isSelf = false)
        {
            if (!string.IsNullOrEmpty(prePipeCode))
            {
                var links   = LineContainer.GetLinkDics();
                var linkKey = string.Concat(prePipeCode, "_", PipeCode);

                if (links.ContainsKey(linkKey))
                {
                    return;
                }
                links.Add(linkKey,new PipeLink()
                {
                    pre_pipe_code = prePipeCode,
                    pipe_code = PipeCode
                });
            }
           
            if (NextPipe == null || Equals(LineContainer.EndPipe))
                return ;

            NextPipe.InterFormatLink(PipeCode,false);
        }


        #endregion

    }
    
}
