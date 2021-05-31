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
using OSS.Tools.DataStack;

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
        ///  管道编码
        /// </summary>
        public string pipe_code { get; set; }

        // 内部异步处理入口
        private readonly IStackPusher<TContext> _pusher ;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pipeType"></param>
        protected BasePipe(PipeType pipeType)
        {
            pipe_type = pipeType;
            _pusher   = DataStackFactory.CreateStack<TContext>(StackPopCaller, "OSS.EventFlow");
        }

        /// <summary>
        /// 启动方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<bool> Start(TContext context)
        {
           return _pusher.Push(context);
        }

        private async Task<bool> StackPopCaller(TContext context)
        {
            var res = await Handling(context);
            if (!res)
            {
                await Block(context);
            }

            return true;
        }
   

        #region 实际管道扩展处理方法
        
        /// <summary>
        ///  管道通过方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal abstract Task<bool> Handling(TContext context);

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

        internal Task<bool> ToNextThrough(OutContext nextInContext)
        {
            return NextPipe != null ? NextPipe.Start(nextInContext) : Task.FromResult(false);
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


        #region MyRegion


        internal override string InterToRoute(string endPipeCode)
        {
            return $"{{ \"pipe_code\":\"{pipe_code}\"" +
                   ",\"pipe_type\":" + (int)pipe_type + (
                       (pipe_code == endPipeCode || NextPipe == null)
                           ? string.Empty
                           : $",\"next\":{NextPipe.InterToRoute(endPipeCode)}") +
                   "}";
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
