using System.Threading.Tasks;
using OSS.EventFlow.Activity.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Activity
{
    /// <summary>
    /// 活动基类 的默认实现
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DefaultActivity<TContext> : BaseActivity<TContext>
        where TContext : IFlowContext
    {
        private readonly IActivityProvider<TContext> _provider;

        /// <inheritdoc />
        public DefaultActivity(IActivityProvider<TContext> provider)
        {
            _provider = provider;
        }

        /// <inheritdoc />
        protected override Task<bool> Executing(TContext data)
        {
            return _provider.Executing(data);
        }
    }
}