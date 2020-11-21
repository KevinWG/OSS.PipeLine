using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Connector
{
    public abstract class BaseSimpleConnector <InContext, OutContext> : BaseConnector<InContext, OutContext>
        where InContext : FlowContext
        where OutContext : FlowContext
    {
    }

    public  class SimpleConnector<TContext> : BaseConnector<TContext, TContext>
        where TContext : FlowContext
    {
        protected override TContext Convert(TContext inContextData)
        {
            return inContextData;
        }
    }
}
