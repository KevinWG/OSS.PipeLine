using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.InterImpls.Pipeline
{
    internal class InterPipelineAppender<TInContext, TOutContext> : IPipelineAppender<TInContext, TOutContext>
    {
        BaseInPipePart<TInContext> IPipelineAppender<TInContext, TOutContext>.StartPipe { get; set; }

        IOutPipeAppender<TOutContext> IPipelineAppender<TInContext, TOutContext>.EndAppender { get; set; }
    }

    internal class
        InterPipelineBranchAppender<TInContext, TOutContext> : IPipelineBranchAppender<TInContext, TOutContext>
    {
        BaseInPipePart<TInContext> IPipelineBranchAppender<TInContext, TOutContext>.    StartPipe   { get; set; }
        BaseBranchGateway<TOutContext> IPipelineBranchAppender<TInContext, TOutContext>.EndAppender { get; set; }
    }
}
