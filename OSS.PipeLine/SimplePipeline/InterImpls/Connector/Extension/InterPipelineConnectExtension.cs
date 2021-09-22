using OSS.Pipeline.Base;

namespace OSS.Pipeline.SimplePipeline.InterImpls.Connector.Extension
{
   internal  static class InterSimplePipelineConnectExtension
    {

        internal static ISimplePipelineConnector<TContext> Set<TContext, TNextResult>(this ISimplePipelineConnector<TContext> connector,
            BaseFourWayPipe<TContext, TContext, TNextResult, TContext> endPipe)
        {
            connector.EndAppender.Append(endPipe);
            connector.EndAppender = endPipe;
            return connector;
        }
        
        //internal static void Set<TContext>(this ISimplePipelineConnector<TContext> connector,
        //    BaseOneWayPipe<TContext> endPipe)
        //{
        //    connector.EndAppender.Append(endPipe);
        //}


        internal static ISimplePipelineBranchConnector<TContext> Set<TContext>(this ISimplePipelineConnector<TContext> connector,
            BaseBranchGateway<TContext> endPipe)
        {
            connector.EndAppender.Append(endPipe);
          
            return new InterSimplePipelineBranchConnector<TContext>(connector.StartPipe, endPipe);
        }

    }
}
