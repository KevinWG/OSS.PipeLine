using System.Threading.Tasks;
using OSS.EventFlow.Activity;
using OSS.EventFlow.Impls.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Impls
{
    /// <summary>
    ///   外部Action活动基类的默认实现
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class DefaultActionActivity<TContext, TResult> : BaseActionActivity<TContext, TResult>
        where TContext : IPipeContext
    {
        private readonly IActionActivityProvider<TContext, TResult> _provider;

        /// <summary>
        ///  外部Action活动基类的默认实现
        /// </summary>
        /// <param name="provider">默认实现的提供者</param>
        public DefaultActionActivity(IActionActivityProvider<TContext, TResult> provider)
        {
            _provider = provider;
        }

        private readonly IActionActivityWithNoticeProvider<TContext, TResult> _nProvider;

        /// <summary>
        ///  外部Action活动基类的默认实现
        /// </summary>
        /// <param name="provider">默认实现的提供者</param>
        public DefaultActionActivity(IActionActivityWithNoticeProvider<TContext, TResult> provider)
        {
            _nProvider = provider;
        }


        /// <inheritdoc />
        protected override Task<TResult> Executing(TContext data, out bool isBlocked)
        {
            return _provider.Executing(data, out isBlocked);
        }

        /// <inheritdoc />
        public override Task<bool> Notice(TContext data)
        {
            return _nProvider?.Notice(data) ?? base.Notice(data);
        }
    }
}