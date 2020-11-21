using System.Threading.Tasks;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Connector
{
    public abstract class BaseConnector<InContext, OutContext> : BaseSinglePipe<InContext, OutContext>
        where InContext : FlowContext
        where OutContext : FlowContext
    {
        protected BaseConnector() : base(PipeType.Connector)
        {
        }
        
        protected abstract OutContext Convert(InContext inContextData);

        internal override Task Through(InContext context)
        {
            var outContext = Convert(context);
            return NextPipe.Through(outContext);
        }
    }


}
