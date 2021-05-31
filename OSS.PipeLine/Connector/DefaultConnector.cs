using System;

namespace OSS.PipeLine.Connector
{
    /// <summary>
    ///  异步缓冲连接器的默认实现
    /// </summary>
    /// <typeparam name="InContext"></typeparam>
    /// <typeparam name="OutContext"></typeparam>
    public class DefaultConnector<InContext, OutContext> : BaseConnector<InContext, OutContext>
    //where InContext : IPipeContext
    //where OutContext : IPipeContext
    {
        private readonly Func<InContext, OutContext> _convert;
        /// <inheritdoc/>
        public DefaultConnector(Func<InContext, OutContext> convertFunc)
        {
            _convert = convertFunc ?? throw new ArgumentNullException(nameof(convertFunc), "转换方法必须传入！");
        }

        /// <inheritdoc/>
        protected override OutContext Convert(InContext inContextData)
        {
            return _convert(inContextData);
        }
    }
}