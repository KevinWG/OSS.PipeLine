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
using OSS.EventFlow.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow
{
    /// <summary>
    /// 基础流体
    /// </summary>
    /// <typeparam name="InFlowContext"></typeparam>
    /// <typeparam name="OutFlowContext"></typeparam>
    public class EventFlow<InFlowContext, OutFlowContext> : BaseSinglePipe<InFlowContext, OutFlowContext>
        where InFlowContext : IPipeContext
        where OutFlowContext : IPipeContext
    {
        /// <summary>
        /// 基础流体
        /// </summary>
        public EventFlow(BasePipe<InFlowContext> startPipe, IPipeAppender<OutFlowContext> endPipeAppender) : base(PipeType.Flow)
        {
            _startPipe = startPipe;
            _endPipeAppender = endPipeAppender;

            if (_startPipe == null)
            {
                throw new ArgumentNullException("未发现流体的起始截止管道！");
            }
        }
        
        private BasePipe<InFlowContext> _startPipe;
        private IPipeAppender<OutFlowContext> _endPipeAppender;

        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal override void InterAppend<NextOutContext>(BaseSinglePipe<OutFlowContext, NextOutContext> nextPipe)
        {
            _endPipeAppender.Append(nextPipe);
        }

        internal override async Task<bool> Through(InFlowContext context)
        {
            await _startPipe.Start(context);
            return true;
        }

        /// <summary>
        ///  生成路径
        /// </summary>
        /// <returns></returns>
        public string ToRoute()
        {
            return _startPipe.InterToRoute(_endPipeAppender.pipe_meta?.pipe_code);
        }
    }

    /// <inheritdoc />
    public class EventFlow<TContext> : EventFlow<TContext, TContext>
        where TContext : IPipeContext
    {
        /// <inheritdoc />
        public EventFlow(BasePipe<TContext> startPipe, IPipeAppender<TContext> endPipeAppender):base(startPipe, endPipeAppender)
        {
        }
    }
    

    /// <summary>
    /// EventFlow 创建工厂
    /// </summary>
    public static class EventFlowExtension
    {
        /// <summary>
        /// 根据首位两个管道建立流体
        /// </summary>
        /// <typeparam name="InFlowContext"></typeparam>
        /// <typeparam name="OutFlowContext"></typeparam>
        /// <param name="startPipe"></param>
        /// <param name="endPipeAppender"></param>
        /// <returns></returns>
        public static EventFlow<InFlowContext, OutFlowContext> AsFlowStartAndEndWith<InFlowContext, OutFlowContext>(
            this BasePipe<InFlowContext> startPipe, IPipeAppender<OutFlowContext> endPipeAppender)
            where InFlowContext : IPipeContext
            where OutFlowContext : IPipeContext
        {
            return new(startPipe, endPipeAppender);
        }


        /// <summary>
        /// 根据首位两个管道建立流体
        /// </summary>
        /// <typeparam name="FlowContext"></typeparam>
        /// <param name="startPipe"></param>
        /// <param name="endPipeAppender"></param>
        /// <returns></returns>
        public static EventFlow<FlowContext> AsFlowStartAndEndWith<FlowContext>(
            this BasePipe<FlowContext> startPipe, IPipeAppender<FlowContext> endPipeAppender)
            where FlowContext : IPipeContext
        {
            return new(startPipe, endPipeAppender);
        }
    }

}
