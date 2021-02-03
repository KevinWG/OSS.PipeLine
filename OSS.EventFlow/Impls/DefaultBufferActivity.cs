using System.Threading.Tasks;
using OSS.EventFlow.Activity.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Activity
{
    /// <summary>
    /// 默认异步消息延缓活动基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DefaultBufferActivity<TContext> : BaseBufferActivity<TContext>
        where TContext : IFlowContext
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