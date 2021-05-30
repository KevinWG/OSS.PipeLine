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

using System;
using System.Threading.Tasks;
using OSS.EventFlow.Connector;
using OSS.EventFlow.Gateway;
using OSS.EventFlow.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow
{
    /// <summary>
    /// 管道基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BasePipe<TContext>: IPipe
        where TContext : IPipeContext
    {
        /// <summary>
        ///  管道类型
        /// </summary>
        public PipeType pipe_type { get; internal set; }

        /// <summary>
        ///  管道元数据信息
        /// </summary>
        public PipeMeta pipe_meta { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pipeType"></param>
        protected BasePipe(PipeType pipeType)
        {
            pipe_type = pipeType;
        }

        /// <summary>
        ///  管道内数据开始流动校验
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task<bool> StartCheck(TContext context)
        {
            return Task.FromResult(true);
        }


        /// <summary>
        ///  管道通过方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal abstract Task<bool> Through(TContext context);

        /// <summary>
        /// 启动方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Start(TContext context)
        {
            var checkRes = await StartCheck(context);
            if (!checkRes)
            {
                return;
            }

            var res = await Through(context);
            if (!res)
            {
                await Block(context);
            }
        }

        /// <summary>
        ///  管道堵塞
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task Block(TContext context)
        {
            return Task.CompletedTask;
        }

        internal abstract string InterToRoute(string endPipeCode);

    }


    /// <summary>
    ///  基础管道
    /// </summary>
    /// <typeparam name="InContext"></typeparam>
    /// <typeparam name="OutContext"></typeparam>
    public abstract class BaseSinglePipe<InContext, OutContext> : BasePipe<InContext>, IPipeAppender<OutContext>
        where InContext : IPipeContext
        where OutContext : IPipeContext
    {
        internal BasePipe<OutContext> NextPipe { get; set; }

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="pipeType"></param>
        protected BaseSinglePipe(PipeType pipeType) : base(pipeType)
        {
        }

        internal Task ToNextThrough(OutContext nextInContext)
        {
            return NextPipe != null ? NextPipe.Start(nextInContext) : Task.CompletedTask;
        }

        /// <summary>
        ///  添加下个管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal virtual void InterAppend<NextOutContext>(BaseSinglePipe<OutContext, NextOutContext> nextPipe)
            where NextOutContext : IPipeContext
        {
            NextPipe = nextPipe;
        }

        /// <summary>
        /// 添加下个管道
        /// </summary>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <param name="nextPipe"></param>
        /// <returns>返回下个管道的追加器</returns>
        public BaseSinglePipe<OutContext, NextOutContext> Append<NextOutContext>(BaseSinglePipe<OutContext, NextOutContext> nextPipe)
            where NextOutContext : IPipeContext
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

        internal override string InterToRoute(string endPipeCode)
        {
            return $"{{ \"pipe_code\":\"{pipe_meta?.pipe_code}\"" +
                $",\"pipe_name\":\"{pipe_meta?.pipe_name}\" " +
                ",\"pipe_type\":" + (int) pipe_type + (
                    (pipe_meta?.pipe_code == endPipeCode || NextPipe == null)
                        ? string.Empty
                        : $",\"next\":{NextPipe.InterToRoute(endPipeCode)}") +
                "}";
        }
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
            where InContext : IPipeContext
            where OutContext : IPipeContext
            where NextOutContext : IPipeContext
        {
            var connector = new DefaultConnector<OutContext, NextOutContext>(convertFunc);
            pipe.InterAppend(connector);
            return connector;
        }


    }



}
