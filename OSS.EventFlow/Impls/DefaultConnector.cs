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
    public class DefaultConnector<InContext, OutContext> : BaseConnector<InContext, OutContext>
        where InContext : IPipeContext
        where OutContext : IPipeContext
    {
        private readonly IConnectorProvider<InContext, OutContext> _provider;

        /// <inheritdoc/>
        public DefaultConnector(IConnectorProvider<InContext, OutContext> provider)
        {
            _provider = provider;
        }


        /// <inheritdoc/>
        protected override OutContext Convert(InContext inContextData)
        {
            return _provider.Convert(inContextData);
        }
    }
}