using System.Collections.Generic;
using OSS.Pipeline.Base;

namespace OSS.Pipeline.Pipeline.InterImpls.Connector.Extension
{
   internal  static class InterPipelineConnectExtension
    {

        internal static IPipelineConnector<TIn, TNextOut> Set<TIn, TOut, TNextPara, TNextResult, TNextOut>(this IPipelineConnector<TIn, TOut> oldAppender,
            BaseFourWayPipe<TOut, TNextPara, TNextResult, TNextOut> endPipe)
        {
            oldAppender.EndAppender.Append(endPipe);
            IPipelineConnector<TIn, TNextOut> pipelineAppender = new InterPipelineConnector<TIn, TNextOut>(oldAppender.StartPipe, endPipe);

            oldAppender.StartPipe = null;
            oldAppender.EndAppender = null;
            return pipelineAppender;
        }


        internal static IPipelineConnector<TIn, TNextOut> Set<TIn, TOut, TNextPara, TNextResult, TNextOut>(this IPipelineConnector<TIn, TOut> oldAppender,
            BaseFourWayPipe<Empty, TNextPara, TNextResult, TNextOut> endPipe)
        {
            oldAppender.EndAppender.Append(endPipe);
            IPipelineConnector<TIn, TNextOut> appender =
                new InterPipelineConnector<TIn, TNextOut>(oldAppender.StartPipe, endPipe);

            oldAppender.StartPipe = null;
            oldAppender.EndAppender = null;
            return appender;
        }

        internal static void Set<TIn, TOut>(this IPipelineConnector<TIn, TOut> oldAppender,
            BaseOneWayPipe<TOut> endPipe)
        {
            oldAppender.EndAppender.Append(endPipe);

            oldAppender.StartPipe = null;
            oldAppender.EndAppender = null;
        }


        internal static IPipelineBranchConnector<TIn, TOut> Set<TIn, TOut>(this IPipelineConnector<TIn, TOut> oldAppender,
            BaseBranchGateway<TOut> endPipe)
        {
            oldAppender.EndAppender.Append(endPipe);
            IPipelineBranchConnector<TIn, TOut> appender =
                new InterPipelineBranchConnector<TIn, TOut>(oldAppender.StartPipe, endPipe);

            return appender;
        }


        internal static IPipelineMsgEnumerableConnector<TIn, TMsgEnumerable, TMsg> Set<TIn, TMsgEnumerable, TMsg>(
            this IPipelineConnector<TIn, TMsgEnumerable> oldAppender,
            BaseMsgEnumerator<TMsgEnumerable, TMsg> endPipe)
            where TMsgEnumerable : IEnumerable<TMsg>
        {
            oldAppender.EndAppender.Append(endPipe);

            var appender =
                new InterPipelineMsgEnumerableConnector<TIn, TMsgEnumerable, TMsg>(oldAppender.StartPipe, endPipe);

            return appender;
        }
    }
}
