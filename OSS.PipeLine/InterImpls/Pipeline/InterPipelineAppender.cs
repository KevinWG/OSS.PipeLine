using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.InterImpls.Pipeline
{
    internal class InterPipelineAppender<TInContext, TOutContext> : IPipelineAppender<TInContext, TOutContext>
    {

        public InterPipelineAppender(BaseInPipePart<TInContext> startPipe, IPipeAppender<TOutContext> endPipe)
        {
            Initial(this, startPipe, endPipe);
        }
        
        private static void Initial(IPipelineAppender<TInContext, TOutContext> pipelineAppender,
            BaseInPipePart<TInContext> startPipe, IPipeAppender<TOutContext> endPipe)
        {
            pipelineAppender.StartPipe   = startPipe;
            pipelineAppender.EndAppender = endPipe;
        }

        BaseInPipePart<TInContext> IPipelineAppender<TInContext, TOutContext>.StartPipe { get; set; }

        IPipeAppender<TOutContext> IPipelineAppender<TInContext, TOutContext>.EndAppender { get; set; }
    }

    internal class
        InterPipelineBranchAppender<TInContext, TOutContext> : IPipelineBranchAppender<TInContext, TOutContext>
    {
        public InterPipelineBranchAppender(BaseInPipePart<TInContext> startPipe, BaseBranchGateway<TOutContext> endPipe)
        {
            Initial(this, startPipe, endPipe);
        }

        private static void Initial(IPipelineBranchAppender<TInContext, TOutContext> pipelineAppender,
            BaseInPipePart<TInContext> startPipe, BaseBranchGateway<TOutContext> endPipe)
        {
            pipelineAppender.StartPipe     = startPipe;
            pipelineAppender.EndBranchPipe = endPipe;
        }

        BaseInPipePart<TInContext> IPipelineBranchAppender<TInContext, TOutContext>.    StartPipe   { get; set; }
        BaseBranchGateway<TOutContext> IPipelineBranchAppender<TInContext, TOutContext>.EndBranchPipe { get; set; }
    }

    //internal class
    //    InterPipelineInterceptAppender<TInContext, TOutContext> : IPipelineInterceptAppender<TInContext, TOutContext>
    //{
    //    public InterPipelineInterceptAppender(BaseInPipePart<TInContext> startPipe, BaseInterceptPipe<TOutContext> endPipe)
    //    {
    //        Initial(this, startPipe, endPipe);
    //    }

    //    private static void Initial(IPipelineInterceptAppender<TInContext, TOutContext> pipelineAppender,
    //        BaseInPipePart<TInContext> startPipe, BaseInterceptPipe<TOutContext> endPipe)
    //    {
    //        pipelineAppender.StartPipe = startPipe;
    //        pipelineAppender.EndPipe   = endPipe;
    //    }
    //    BaseInPipePart<TInContext> IPipelineInterceptAppender<TInContext, TOutContext>.    StartPipe     { get; set; }
    //    BaseInterceptPipe<TOutContext> IPipelineInterceptAppender<TInContext, TOutContext>.EndPipe { get; set; }
    //}
}
