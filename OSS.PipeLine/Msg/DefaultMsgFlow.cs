using System;

namespace OSS.Pipeline.Msg
{

    /// <inheritdoc />
    public class DefaultMsgFlow<TContext> : BaseMsgFlow<TContext>
    {
        /// <inheritdoc />
        public DefaultMsgFlow(string msgDataFlowKey) : base(msgDataFlowKey)
        {

        }
    }


    /// <summary>
    ///  内部转化连接器的实现
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TOutContext"></typeparam>
    public class DefaultMsgConvertor<TInContext, TOutContext> : BaseMsgConvertor<TInContext, TOutContext>
    {
        private readonly Func<TInContext, TOutContext> _convert;
        /// <inheritdoc/>
        public DefaultMsgConvertor(Func<TInContext, TOutContext> convertFunc)
        {
            _convert = convertFunc ?? throw new ArgumentNullException(nameof(convertFunc), "转换方法必须传入！");
        }

        /// <inheritdoc/>
        protected override TOutContext Convert(TInContext inContextData)
        {
            return _convert(inContextData);
        }
    }



}