using System;

namespace OSS.Pipeline.Connector
{
    /// <summary>
    ///  缓冲连接器的默认实现
    /// </summary>
    /// <typeparam name="InContext"></typeparam>
    /// <typeparam name="OutContext"></typeparam>
    public class DefaultConnector<InContext, OutContext> : BaseConnector<InContext, OutContext>
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


    /// <summary>
    ///  异步缓冲转化连接器的默认实现
    /// </summary>
    /// <typeparam name="InContext"></typeparam>
    /// <typeparam name="OutContext"></typeparam>
    public class DefaultBufferConnector<InContext, OutContext> : BaseBufferConnector<InContext, OutContext>
    {
        private readonly Func<InContext, OutContext> _convert;
        /// <inheritdoc/>
        public DefaultBufferConnector(Func<InContext, OutContext> convertFunc)
        {
            _convert = convertFunc ?? throw new ArgumentNullException(nameof(convertFunc), "转换方法必须传入！");
        }

        /// <inheritdoc/>
        protected override OutContext Convert(InContext inContextData)
        {
            return _convert(inContextData);
        }
    }

    /// <summary>
    ///  异步缓冲连接器的默认实现
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DefaultBufferConnector<TContext> : BaseBufferConnector<TContext>
    {
        /// <inheritdoc />
        public DefaultBufferConnector() 
        {
        }
    }
}