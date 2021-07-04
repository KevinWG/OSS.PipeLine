using System.Collections.Generic;
using OSS.Pipeline.Base;

namespace OSS.Pipeline.Pipeline.InterImpls.Connector
{
    internal class InterPipelineMsgEnumerableConnector<TInContext, TMsgEnumerable, TMsg> : IPipelineMsgEnumerableConnector<TInContext, TMsgEnumerable, TMsg>
        where TMsgEnumerable:IEnumerable<TMsg> 
    {
        public InterPipelineMsgEnumerableConnector(BaseInPipePart<TInContext> startPipe, BaseMsgEnumerator<TMsgEnumerable, TMsg> endPipe)
        {
            Initial(this, startPipe, endPipe);
        }

        private static void Initial(IPipelineMsgEnumerableConnector<TInContext, TMsgEnumerable, TMsg> pipelineAppender,
            BaseInPipePart<TInContext> startPipe, BaseMsgEnumerator<TMsgEnumerable, TMsg> endPipe)
        {
            pipelineAppender.StartPipe = startPipe;
            pipelineAppender.EndPipe   = endPipe;
        }

        BaseInPipePart<TInContext> IPipelineMsgEnumerableConnector<TInContext, TMsgEnumerable, TMsg>.    StartPipe     { get; set; }
        BaseMsgEnumerator<TMsgEnumerable, TMsg> IPipelineMsgEnumerableConnector<TInContext, TMsgEnumerable, TMsg>.EndPipe { get; set; }
    }
}