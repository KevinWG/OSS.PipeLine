#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventTask - 流体基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-28
*       
*****************************************************************************/

#endregion

using System;
using System.Threading.Tasks;
using OSS.EventFlow.Connector;
using OSS.EventFlow.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow
{
    /// <summary>
    /// 基础流体
    /// </summary>
    /// <typeparam name="InFlowContext"></typeparam>
    /// <typeparam name="OutFlowContext"></typeparam>
    public abstract class BaseFlow<InFlowContext, OutFlowContext> : BaseSinglePipe<InFlowContext, OutFlowContext>
        where InFlowContext : FlowContext
        where OutFlowContext : FlowContext
    {
        /// <summary>
        /// 基础流体
        /// </summary>
        protected BaseFlow() : base(PipeType.Flow)
        {
        }

        /// <summary>
        ///  开始管道
        /// </summary>
        public BasePipe<InFlowContext> StartPipe { get; internal set; }

        /// <summary>
        ///  流体开始
        /// </summary>
        /// <param name="startPipe"></param>
        public void Start(BasePipe<InFlowContext> startPipe)
        {
            StartPipe = startPipe;
        }

        /// <summary>
        ///  开始管道
        /// </summary>
        private IPipeAppender<OutFlowContext> _endPipeAppender;

        /// <summary>
        ///  流体开始
        /// </summary>
        /// <param name="endPipeAppender"></param>
        public void End(IPipeAppender<OutFlowContext> endPipeAppender)
        {
            _endPipeAppender = endPipeAppender;
        }

        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal override void InterAppend(BasePipe<OutFlowContext> nextPipe)
        {
            _endPipeAppender.Append(nextPipe);
        }


        internal override Task Through(InFlowContext context)
        {
            return StartPipe.Through(context);
        }
    }
}
