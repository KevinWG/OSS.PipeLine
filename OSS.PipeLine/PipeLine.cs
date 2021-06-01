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
using OSS.PipeLine.Interface;
using OSS.PipeLine.Mos;

namespace OSS.PipeLine
{
    /// <summary>
    /// 基础流体
    /// </summary>
    /// <typeparam name="InFlowContext"></typeparam>
    /// <typeparam name="OutFlowContext"></typeparam>
    public class PipeLine<InFlowContext, OutFlowContext> : BasePipe<InFlowContext, OutFlowContext>, IFlow
    {
        /// <summary>
        /// 基础流体
        /// </summary>
        public PipeLine(BasePipePart<InFlowContext> startPipe, BaseSinglePipe<OutFlowContext> endPipeAppender) : base(PipeType.Flow)
        {
            _startPipe = startPipe;
            _endPipe = endPipeAppender;

            if (_startPipe == null || _endPipe == null)
            {
                throw new ArgumentNullException("未发现流体的起始截止管道！");
            }

            startPipe.InterInitialContainer(this);
        }

        public readonly BasePipePart<InFlowContext>    _startPipe;
        public readonly BaseSinglePipe<OutFlowContext> _endPipe;

        /// <summary>
        ///  开始管道
        /// </summary>
        public IPipe StartPipe => _startPipe;
        /// <summary>
        ///  结束管道
        /// </summary>
        public IPipe EndPipe => _endPipe;


        internal override async Task<bool> InterHandling(InFlowContext context)
        {
            await _startPipe.Start(context);
            return true;
        }


        /// <summary>
        ///  当前流的路由信息
        /// </summary>
        private PipeRoute _route;

        /// <summary>
        ///  生成路径
        /// </summary>
        /// <returns></returns>
        public PipeRoute ToRoute()
        {
            return _route ??= InterToRoute();
        }

        #region 内部的扩散方法

        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal override void InterAppend<NextOutContext>(BasePipe<OutFlowContext, NextOutContext> nextPipe)
        {
            NextPipe = nextPipe;
            _endPipe.Append(nextPipe);
        }

        internal override PipeRoute InterToRoute()
        {
            var pipe = new PipeRoute
            {
                pipe_code = PipeCode, pipe_type = PipeType, inter_pipe = _startPipe.InterToRoute()
            };

            if (NextPipe != null)
            {
                pipe.next = NextPipe.InterToRoute();
            }
            return pipe;
        }

        #endregion

    }

    /// <inheritdoc />
    public class PipeLine<TContext> : PipeLine<TContext, TContext>
    {
        /// <inheritdoc />
        public PipeLine(BasePipePart<TContext> startPipe, BaseSinglePipe<TContext> endPipeAppender) : base(startPipe, endPipeAppender)
        {
        }
    }

    /// <summary>
    /// EventFlow 创建工厂
    /// </summary>
    public static class PipeLineExtension
    {
        /// <summary>
        /// 根据首位两个管道建立流体
        /// </summary>
        /// <typeparam name="InFlowContext"></typeparam>
        /// <typeparam name="OutFlowContext"></typeparam>
        /// <param name="startPipe"></param>
        /// <param name="endPipeAppender"></param>
        /// <param name="flowPipeCode"></param>
        /// <returns></returns>
        public static PipeLine<InFlowContext, OutFlowContext> AsFlowStartAndEndWith<InFlowContext, OutFlowContext>(
            this BasePipePart<InFlowContext> startPipe, BaseSinglePipe<OutFlowContext> endPipeAppender, string flowPipeCode = null)
        {
            return new(startPipe, endPipeAppender) { PipeCode = flowPipeCode ?? string.Concat(startPipe.PipeCode, "Flow") };
        }
    }


}
