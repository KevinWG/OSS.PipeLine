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
    /// <typeparam name="InContext"></typeparam>
    /// <typeparam name="OutContext"></typeparam>
    public abstract class BaseFlow<InContext, OutContext> : BaseSinglePipe<InContext, OutContext>
        where InContext : FlowContext
        where OutContext : FlowContext
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
        public BasePipe<InContext> StartPipe { get; internal set; }

        /// <summary>
        ///  流体开始
        /// </summary>
        /// <param name="startPipe"></param>
        public void Start(BasePipe<InContext> startPipe)
        {
            StartPipe = startPipe;
        }

        /// <summary>
        ///  开始管道
        /// </summary>
        private INextPipeAppender<OutContext> _endPipeAppender;

        /// <summary>
        ///  流体开始
        /// </summary>
        /// <param name="endPipeAppender"></param>
        public void End(INextPipeAppender<OutContext> endPipeAppender)
        {
            _endPipeAppender = endPipeAppender;
        }

        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal override void InterAppend(BasePipe<OutContext> nextPipe)
        {
            _endPipeAppender.Append(nextPipe);
        }

        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道
        /// </summary>
        /// <param name="nextPipe"></param>
        /// <param name="connectorAfterNextPipe"></param>
        public void Append<TNextConnectorOutContext>(BasePipe<OutContext> nextPipe,
            BaseConnector<OutContext, TNextConnectorOutContext> connectorAfterNextPipe)
            where TNextConnectorOutContext : FlowContext
        {
            //todo

        }


        internal override Task Through(InContext context)
        {
            return StartPipe.Through(context);
        }
    }
}
