using System.Threading.Tasks;
using OSS.EventFlow.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Activity
{
    public abstract class BaseBlockActivity<TContext> : BaseActivity<TContext>,IBlockTunnel<TContext,TContext>
        where TContext : FlowContext
    {
        public abstract Task Push(TContext data);

        public abstract Task Pop(TContext data);
    }
}
