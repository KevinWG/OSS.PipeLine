using System.Threading.Tasks;
using OSS.EventFlow.Connector;
using OSS.EventFlow.Impls.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Impls
{
    /// <summary>
    ///  异步缓冲连接器的默认实现
    /// </summary>
    /// <typeparam name="InContext"></typeparam>
    /// <typeparam name="OutContext"></typeparam>
    public class DefaultBufferConnector<InContext, OutContext> : BaseBufferConnector<InContext, OutContext>
        where InContext : IPipeContext
        where OutContext : IPipeContext
    {
        private readonly IBufferConnectorProvider<InContext, OutContext> _provider;

        /// <inheritdoc/>
        public DefaultBufferConnector(IBufferConnectorProvider<InContext, OutContext> provider)
        {
            _provider = provider;
        }

        /// <inheritdoc/>
        public override Task<bool> Push(InContext data)
        {
            return _provider.Push(data);
        }

        /// <inheritdoc/>
        protected override OutContext Convert(InContext inContextData)
        {
            return _provider.Convert(inContextData);
        }
    }

    /// <inheritdoc />
    public class DefaultBufferConnector<TContext> : DefaultBufferConnector<TContext, TContext>
        where TContext : IPipeContext
    {
        /// <inheritdoc />
        public DefaultBufferConnector(IBufferConnectorProvider<TContext> provider) : base(provider)
        {
        }
    }
}