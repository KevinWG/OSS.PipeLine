#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  流体扩展
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion


using System;
using System.Collections.Generic;
using OSS.DataFlow;
using OSS.Pipeline.Interface;
using OSS.Pipeline.InterImpls.Msg;

namespace OSS.Pipeline
{
    /// <summary>
    ///  pipeline 生成器
    /// </summary>
    public static partial class PipelineExtension
    {
        /// <summary>
        ///  追加默认消息订阅者管道
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="pipeCode">消息pipeDataKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IPipelineConnector<TIn, TOut> ThenWithMsgSubscriber<TIn, TOut>(
            this IPipelineConnector<TIn, TOut> pipe,
            string pipeCode, DataFlowOption option = null)
        {
            return pipe.Then(new MsgSubscriber<TOut>(pipeCode, option));
        }


        /// <summary>
        ///  追加默认消息流管道
        /// </summary>
        /// <param name="pipe"></param>
        /// <param name="pipeCode">消息pipeDataKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IPipelineConnector<TIn, TOut> ThenWithMsgFlow<TIn, TOut>(
            this IPipelineConnector<TIn, TOut> pipe, 
            string pipeCode, DataFlowOption option = null)
        {
            var nextPipe = new MsgFlow<TOut>(pipeCode, option);

            return pipe.Then(nextPipe);
        }

        /// <summary>
        ///  追加默认消息转换管道
        /// </summary>
        /// <param name="pipe"></param>
        /// <param name="convertFunc"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static IPipelineConnector<TIn, TNextOut> ThenWithMsgConverter<TIn, TOut, TNextOut>(
            this IPipelineConnector<TIn, TOut> pipe, 
            Func<TOut, TNextOut> convertFunc, string pipeCode = null)
        {
            var nextPipe = new InterMsgConvertor<TOut, TNextOut>(pipeCode, convertFunc);
            return pipe.Then(nextPipe);
        }


        /// <summary>
        ///  追加消息枚举器
        /// </summary>
        /// <param name="pipe"></param>
        /// <param name="pipeCode"></param>
        /// <typeparam name="TMsg">消息具体类型</typeparam>
        /// <typeparam name="TMsgEnumerable">消息的枚举类型如 IList&lt;TMsg&gt;</typeparam>
        /// <typeparam name="TIn"></typeparam>
        /// <returns></returns>
        public static IPipelineMsgEnumerableConnector<TIn, TMsgEnumerable, TMsg> ThenWithMsgEnumerator<TIn, TMsgEnumerable, TMsg>(
            this IPipelineConnector<TIn, TMsgEnumerable> pipe, string pipeCode=null)
            where TMsgEnumerable : IEnumerable<TMsg>
        {
            var nextPipe = new BaseMsgEnumerator<TMsgEnumerable, TMsg>(pipeCode);
            return pipe.Then(nextPipe);
        }
    }
}
