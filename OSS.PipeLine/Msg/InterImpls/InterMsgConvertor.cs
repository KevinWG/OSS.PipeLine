#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  消息内部实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion
using System;

namespace OSS.Pipeline.InterImpls.Msg
{
    /// <summary>
    ///  内部转化连接器的实现
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    internal class InterMsgConvertor<TIn, TOut> : BaseMsgConverter<TIn, TOut>
    {
        private readonly Func<TIn, TOut> _convert;
        
        /// <inheritdoc/>
        public InterMsgConvertor( string pipeCode,Func<TIn, TOut> convertFunc):base(pipeCode)
        {
            _convert = convertFunc ?? throw new ArgumentNullException(nameof(convertFunc), "转换方法必须传入！");
        }

        /// <inheritdoc/>
        protected override TOut Convert(TIn inContextData)
        {
            return _convert(inContextData);
        }
    }



}