using OSS.Pipeline.Base;
using OSS.Pipeline.Pipeline.InterImpls.Connector;

namespace OSS.Pipeline.SimplePipeline.InterImpls.Connector
{
    internal class InterSimplePipelineBranchConnector<TContext> 
        : InterPipelineBranchConnector<TContext,TContext>, ISimplePipelineBranchConnector<TContext>
    {
        public InterSimplePipelineBranchConnector(BaseInPipePart<TContext> startPipe, BaseBranchGateway<TContext> endPipe):base(startPipe,endPipe)
        {
        }

    }
}