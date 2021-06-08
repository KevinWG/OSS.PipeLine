using System;

namespace OSS.Pipeline.InterImpls.Msg
{
    /// <summary>
    ///  内部转化连接器的实现
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TOutContext"></typeparam>
    internal class InterMsgConvertor<TInContext, TOutContext> : BaseMsgConverter<TInContext, TOutContext>
    {
        private readonly Func<TInContext, TOutContext> _convert;
        /// <inheritdoc/>
        public InterMsgConvertor(Func<TInContext, TOutContext> convertFunc,string pipeCode)
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