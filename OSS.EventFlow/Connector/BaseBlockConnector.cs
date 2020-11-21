using System;
using System.Threading.Tasks;
using OSS.EventFlow.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Connector
{
    public abstract class BaseBlockConnector<InContext, OutContext> : BaseConnector<InContext, OutContext>,
        IBlockTunnel<InContext, OutContext>
        where InContext : FlowContext
        where OutContext : FlowContext
    {
        public abstract Task Push(InContext data);

        public abstract Task Pop(OutContext data);
    }


    public abstract class BaseBlockConnector<TContext> : BaseBlockConnector<TContext, TContext>,
        IBlockTunnel<TContext, TContext>
        where TContext : FlowContext
    {
        protected override TContext Convert(TContext inContextData)
        {
            return inContextData;
        }
    }
}
