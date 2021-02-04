using System.Threading.Tasks;
using OSS.EventFlow.Activity;
using OSS.EventFlow.Impls.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Impls
{
    /// <summary>
    /// 默认异步消息延缓活动基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DefaultBufferActivity<TContext> : BaseBufferActivity<TContext>
        where TContext : IPipeContext
    {

        private readonly IBufferActivityProvider<TContext> _provider;
        /// <summary>
        /// 异步消息延缓活动基类
        /// </summary>
        /// <param name="provider"></param>
        public DefaultBufferActivity(IBufferActivityProvider<TContext> provider)
        {
            _provider = provider;
        }

        /// <inheritdoc />
        public override Task<bool> Push(TContext data)
        {
            return _provider.Push(data);
        }

        /// <inheritdoc />
        protected override Task<bool> Executing(TContext data)
        {
            return _provider.Executing(data);
        }
    }
}