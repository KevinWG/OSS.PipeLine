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
            _startPipe       = InitialInnerStartPipe();
            _endPipeAppender = InitialInnerEndPipe();

            if (_startPipe == null || _endPipeAppender == null)
            {
                throw new ArgumentNullException("未发现流体的起始截止管道！");
            }
        }


        private BasePipe<InFlowContext> _startPipe;
        private IPipeAppender<OutFlowContext> _endPipeAppender;

        /// <summary>
        ///  初始化流体的起始管道
        /// </summary>
        /// <returns></returns>
        protected abstract BasePipe<InFlowContext> InitialInnerStartPipe();

        /// <summary>
        ///  初始化流体的结束管道
        /// </summary>
        /// <returns></returns>
        protected abstract IPipeAppender<OutFlowContext> InitialInnerEndPipe();

        /// <summary>
        ///    触发
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Trigger(InFlowContext context)
        {
            return InternalDeepThrough(context);
        }

        /// <summary>
        ///  链接流体内部尾部管道和流体外下一截管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal override void InterAppend(BasePipe<OutFlowContext> nextPipe)
        {
            _endPipeAppender.Append(nextPipe);
        }

        internal override async Task<bool> Through(InFlowContext context)
        {
            // 内部子管道的阻塞传递给父级
            await _startPipe.InternalDeepThrough(context);
            return true;
        }
    }
}
