#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  流体追加器内部实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.InterImpls.Pipeline
{
    internal class InterSimplePipelineAppender<TContext> : ISimplePipelineAppender<TContext>
    {

        public InterSimplePipelineAppender(BaseInPipePart<TContext> startPipe, IPipeAppender<TContext> endPipe)
        {
            Initial(this, startPipe, endPipe);
        }
        
        private static void Initial(ISimplePipelineAppender<TContext> pipelineAppender,
            BaseInPipePart<TContext> startPipe, IPipeAppender<TContext> endPipe)
        {
            pipelineAppender.StartPipe   = startPipe;
            pipelineAppender.EndAppender = endPipe;
        }

        BaseInPipePart<TContext> ISimplePipelineAppender<TContext>.StartPipe { get; set; }

        IPipeAppender<TContext> ISimplePipelineAppender<TContext>.EndAppender { get; set; }
    }

    internal class InterSimplePipelineBranchAppender<TContext> : ISimplePipelineBranchAppender<TContext>
    {
        public InterSimplePipelineBranchAppender(BaseInPipePart<TContext> startPipe, BaseBranchGateway<TContext> endPipe)
        {
            Initial(this, startPipe, endPipe);
        }

        private static void Initial(ISimplePipelineBranchAppender<TContext> pipelineAppender,
            BaseInPipePart<TContext> startPipe, BaseBranchGateway<TContext> endPipe)
        {
            pipelineAppender.StartPipe     = startPipe;
            pipelineAppender.EndBranchPipe = endPipe;
        }

        BaseInPipePart<TContext> ISimplePipelineBranchAppender<TContext>.  StartPipe     { get; set; }
        BaseBranchGateway<TContext> ISimplePipelineBranchAppender<TContext>.EndBranchPipe { get; set; }
    }
}
