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

using OSS.Pipeline.Connector;
using OSS.Pipeline.Gateway;
using OSS.Pipeline.Interface;
using OSS.Pipeline.Mos;
using System;
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <summary>
    ///  基础管道
    /// </summary>
    /// <typeparam name="InContext"></typeparam>
    /// <typeparam name="OutContext"></typeparam>
    public abstract class BasePipe<InContext, OutContext> : BasePipePart<InContext>, IPipeAppender<OutContext>
    {
        internal BasePipePart<OutContext> NextPipe { get; set; }

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="pipeType"></param>
        protected BasePipe(PipeType pipeType) : base(pipeType)
        {
        }

        internal Task<bool> ToNextThrough(OutContext nextInContext)
        {
            return NextPipe != null ? NextPipe.Start(nextInContext) : Task.FromResult(false);
        }


        #region 管道连接处理

        /// <summary>
        ///  添加下个管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal virtual void InterAppend<NextOutContext>(BasePipe<OutContext, NextOutContext> nextPipe)
        {
            NextPipe = nextPipe;
        }

        void IPipeAppender<OutContext>.InterAppend<NextOutContext>(BasePipe<OutContext, NextOutContext> nextPipe)
        {
            InterAppend(nextPipe);
        }

        BaseBranchGateway<OutContext> IPipeAppender<OutContext>.InterAppend(BaseBranchGateway<OutContext> nextPipe)
        {
            InterAppend<OutContext>(null); // 保证 PipeLine 的 InterAppend 可以正常执行
            NextPipe = nextPipe;
            return nextPipe;
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
    ///  基础管道
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseSinglePipe<TContext> : BasePipe<TContext, TContext>
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="pipeType"></param>
        protected BaseSinglePipe(PipeType pipeType) : base(pipeType)
        {
        }
    }


    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static class BaseSinglePipeExtension
    {
        /// <summary>
        ///  追加管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static BasePipe<OutContext, NextOutContext> Append<OutContext, NextOutContext>(
            this IPipeAppender<OutContext> pipe,
            BasePipe<OutContext, NextOutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        /// 追加分支网关管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static BaseBranchGateway<OutContext> Append<OutContext>(
            this IPipeAppender<OutContext> pipe,
            BaseBranchGateway<OutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加 数据转换管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="convertFunc"></param>
        /// <returns></returns>
        public static DefaultConnector<OutContext, NextOutContext> Append<OutContext, NextOutContext>(
            this IPipeAppender<OutContext> pipe,
            Func<OutContext, NextOutContext> convertFunc)
        {
            var connector = new DefaultConnector<OutContext, NextOutContext>(convertFunc);
            pipe.InterAppend(connector);
            return connector;
        }

        /// <summary>
        ///  追加异步流缓冲数据转换组件
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="convertFunc"></param>
        /// <returns></returns>
        public static DefaultBufferConnector<OutContext, NextOutContext> AppendBuffer<OutContext, NextOutContext>(
            this IPipeAppender<OutContext> pipe,
            Func<OutContext, NextOutContext> convertFunc)
        {
            var connector = new DefaultBufferConnector<OutContext, NextOutContext>(convertFunc);
            pipe.InterAppend(connector);
            return connector;
        }


        /// <summary>
        ///  追加异步流缓冲组件
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <returns></returns>
        public static DefaultBufferConnector<OutContext> AppendBuffer<OutContext>(
            this IPipeAppender<OutContext> pipe)
        {
            var connector = new DefaultBufferConnector<OutContext>();
            pipe.InterAppend(connector);
            return connector;
        }
    }
    
}
