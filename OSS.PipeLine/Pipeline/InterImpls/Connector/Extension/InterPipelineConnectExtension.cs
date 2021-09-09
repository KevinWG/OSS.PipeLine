using System.Collections.Generic;
using OSS.Pipeline.Base;

namespace OSS.Pipeline.Pipeline.InterImpls.Connector.Extension
{
    internal static class InterPipelineConnectExtension
    {

        internal static IPipelineConnector<TIn, TNextOut> Set<TIn, TOut, TNextPara, TNextResult, TNextOut>(
            this IPipelineConnector<TIn, TOut> oldConnector,
            BaseFourWayPipe<TOut, TNextPara, TNextResult, TNextOut> endPipe)
        {
            oldConnector.EndAppender.Append(endPipe);
            IPipelineConnector<TIn, TNextOut> pipelineAppender =
                new InterPipelineConnector<TIn, TNextOut>(oldConnector.StartPipe, endPipe);

            oldConnector.StartPipe   = null;
            oldConnector.EndAppender = null;
            return pipelineAppender;
        }


        internal static IPipelineConnector<TIn, TNextOut> Set<TIn, TOut, TNextPara, TNextResult, TNextOut>(
            this IPipelineConnector<TIn, TOut> oldConnector,
            BaseFourWayPipe<Empty, TNextPara, TNextResult, TNextOut> endPipe)
        {
            oldConnector.EndAppender.Append(endPipe);
            IPipelineConnector<TIn, TNextOut> appender =
                new InterPipelineConnector<TIn, TNextOut>(oldConnector.StartPipe, endPipe);

            oldConnector.StartPipe   = null;
            oldConnector.EndAppender = null;
            return appender;
        }

        internal static void Set<TIn, TOut>(this IPipelineConnector<TIn, TOut> oldConnector,
            BaseOneWayPipe<TOut> endPipe)
        {
            oldConnector.EndAppender.Append(endPipe);

            oldConnector.StartPipe   = null;
            oldConnector.EndAppender = null;
        }


        internal static IPipelineBranchConnector<TIn, TOut> Set<TIn, TOut>(
            this IPipelineConnector<TIn, TOut> oldConnector,
            BaseBranchGateway<TOut> endPipe)
        {
            oldConnector.EndAppender.Append(endPipe);
            IPipelineBranchConnector<TIn, TOut> appender =
                new InterPipelineBranchConnector<TIn, TOut>(oldConnector.StartPipe, endPipe);

            return appender;
        }


    }
}
