using OSS.Pipeline.Base;

namespace OSS.Pipeline.Pipeline.InterImpls.Connector
{
    internal class
        InterPipelineMsgEnumerableConnector<TInContext, TMsg> : IPipelineMsgEnumerableConnector<TInContext, TMsg>
    {
        public InterPipelineMsgEnumerableConnector(BaseInPipePart<TInContext> startPipe, MsgEnumerator<TMsg> endPipe)
        {
            Initial(this, startPipe, endPipe);
        }

        private static void Initial(IPipelineMsgEnumerableConnector<TInContext, TMsg> pipelineAppender,
            BaseInPipePart<TInContext> startPipe, MsgEnumerator<TMsg> endPipe)
        {
            pipelineAppender.StartPipe = startPipe;
            pipelineAppender.EndPipe   = endPipe;
        }

        BaseInPipePart<TInContext> IPipelineMsgEnumerableConnector<TInContext, TMsg>.StartPipe { get; set; }
        MsgEnumerator<TMsg> IPipelineMsgEnumerableConnector<TInContext, TMsg>.       EndPipe   { get; set; }
    }
}