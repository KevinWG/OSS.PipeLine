using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Activity
{
    public abstract class BaseActivity<TContext> : BaseSinglePipe<TContext>
        where TContext : FlowContext
    {
        protected BaseActivity() : base(PipeType.Activity)
        {
        }
    }
}
