using OSS.Pipeline.Base;

namespace OSS.Pipeline.Pipeline.InterImpls.Connector
{
    internal class InterPipelineBranchConnector<TInContext, TOutContext> 
        : IPipelineBranchConnector<TInContext, TOutContext>
    {
        public InterPipelineBranchConnector(BaseInPipePart<TInContext> startPipe, BaseBranchGateway<TOutContext> endPipe)
        {
            Initial(this, startPipe, endPipe);
        }

        private static void Initial(IPipelineBranchConnector<TInContext, TOutContext> pipelineAppender,
            BaseInPipePart<TInContext> startPipe, BaseBranchGateway<TOutContext> endPipe)
        {
            pipelineAppender.StartPipe     = startPipe;
            pipelineAppender.EndBranchPipe = endPipe;
        }

        BaseInPipePart<TInContext> IPipelineBranchConnector<TInContext, TOutContext>.    StartPipe     { get; set; }
        BaseBranchGateway<TOutContext> IPipelineBranchConnector<TInContext, TOutContext>.EndBranchPipe { get; set; }
    }
}