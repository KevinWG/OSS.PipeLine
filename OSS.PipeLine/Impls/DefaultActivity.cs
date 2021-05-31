using System.Threading.Tasks;
using OSS.EventFlow.Activity;
using OSS.EventFlow.Impls.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Impls
{
    /// <summary>
    /// 活动基类 的默认实现
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DefaultActivity<TContext> : BaseActivity<TContext>
        //where TContext : IPipeContext
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