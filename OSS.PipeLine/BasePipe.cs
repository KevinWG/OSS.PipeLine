﻿#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 流体基础管道部分
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using OSS.PipeLine.Connector;
using OSS.PipeLine.Gateway;
using OSS.PipeLine.Interface;
using OSS.PipeLine.Mos;
using System;
using System.Threading.Tasks;

namespace OSS.PipeLine
{
    /// <summary>
    /// 管道基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BasePipe<TContext> : IPipe
    {

        /// <summary>
        ///  管道类型
        /// </summary>
        public PipeType PipeType { get; internal set; }

        /// <summary>
        ///  管道编码
        ///  默认等于  this.GetType().Name
        /// </summary>
        public string PipeCode { get; set; }

        /// <summary>
        ///  流容器
        /// </summary>
        protected IPipe FlowContainer { get; set; }

        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pipeType"></param>
        protected BasePipe(PipeType pipeType)
        {
            PipeType = pipeType;
            PipeCode = GetType().Name;
        }
        
        #region 流体启动和异步处理逻辑
        
        /// <summary>
        /// 启动方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> Start(TContext context)
        {
            var res = await InterHandling(context);
            if (!res)
            {
                await Block(context);
            }
            return true;
        }

        #endregion


        #region 实际管道扩展处理方法

        /// <summary>
        ///  管道堵塞
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task Block(TContext context)
        {
            return Task.CompletedTask;
        }

        #endregion


        #region 内部扩散方法

        /// <summary>
        ///  管道处理实际业务流动方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal abstract Task<bool> InterHandling(TContext context);


        /// <summary>
        ///  内部处理流容器初始化赋值
        /// </summary>
        /// <param name="containerFlow"></param>
        internal abstract void InterInitialContainer(IFlow containerFlow);

        /// <summary>
        ///  内部处理流的路由信息
        /// </summary>
        /// <returns></returns>
        internal abstract PipeRoute InterToRoute();

        #endregion

    }


    /// <summary>
    ///  基础管道
    /// </summary>
    /// <typeparam name="InContext"></typeparam>
    /// <typeparam name="OutContext"></typeparam>
    public abstract class BaseSinglePipe<InContext, OutContext> : BasePipe<InContext>, IPipeAppender<OutContext>
    {
        internal BasePipe<OutContext> NextPipe { get; set; }

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="pipeType"></param>
        protected BaseSinglePipe(PipeType pipeType) : base(pipeType)
        {
        }

        internal Task<bool> ToNextThrough(OutContext nextInContext)
        {
            return NextPipe != null ? NextPipe.Start(nextInContext) : Task.FromResult(false);
        }


        #region 管道连接处理

        /// <summary>
        /// 添加下个管道
        /// </summary>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <param name="nextPipe"></param>
        /// <returns>返回下个管道的追加器</returns>
        public BaseSinglePipe<OutContext, NextOutContext> Append<NextOutContext>(BaseSinglePipe<OutContext, NextOutContext> nextPipe)
        //where NextOutContext : IPipeContext
        {
            InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加分支网关
        /// </summary>
        /// <param name="nextPipe"></param>
        public BaseBranchGateway<OutContext> Append(BaseBranchGateway<OutContext> nextPipe)
        {
            NextPipe = nextPipe;
            return nextPipe;
        }


        #endregion


        #region 内部扩散方法

        /// <summary>
        ///  添加下个管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal virtual void InterAppend<NextOutContext>(BaseSinglePipe<OutContext, NextOutContext> nextPipe)
        //where NextOutContext : IPipeContext
        {
            NextPipe = nextPipe;
        }

        internal override void InterInitialContainer(IFlow flowContainer)
        {
            FlowContainer = flowContainer;
            if (this.Equals(flowContainer.EndPipe))
                return;

            if (NextPipe == null)
                throw new ArgumentNullException(nameof(NextPipe), $"Flow({flowContainer.PipeCode})需要有明确的EndPipe，且所有的分支路径最终需到达此EndPipe");

            NextPipe.InterInitialContainer(flowContainer);
        }

        internal override PipeRoute InterToRoute()
        {
            var pipe = new PipeRoute()
            {
                pipe_code = PipeCode,
                pipe_type = PipeType
            };

            if (NextPipe != null)
            {
                pipe.next = NextPipe.InterToRoute();
            }
            return pipe;
        }

        #endregion

    }


    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static class BaseSinglePipeExtension
    {
        /// <summary>
        ///  追加上下文转换组件
        /// </summary>
        /// <typeparam name="InContext"></typeparam>
        /// <typeparam name="OutContext"></typeparam>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="convertFunc"></param>
        /// <returns></returns>
        public static IPipeAppender<NextOutContext> AppendConvert<InContext, OutContext, NextOutContext>(
            this BaseSinglePipe<InContext, OutContext> pipe,
            Func<OutContext, NextOutContext> convertFunc)
        //where InContext : IPipeContext
        //where OutContext : IPipeContext
        //where NextOutContext : IPipeContext
        {
            var connector = new DefaultConnector<OutContext, NextOutContext>(convertFunc);
            pipe.InterAppend(connector);
            return connector;
        }


    }



}