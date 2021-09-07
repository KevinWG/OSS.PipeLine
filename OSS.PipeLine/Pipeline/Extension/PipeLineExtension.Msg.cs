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
using OSS.Pipeline.Base;
using OSS.Pipeline.InterImpls.Msg;
using OSS.Pipeline.Pipeline.InterImpls.Connector;
using OSS.Pipeline.Pipeline.InterImpls.Connector.Extension;

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
            return pipe.Then(new SimpleMsgSubscriber<TOut>(pipeCode, option));
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
            var nextPipe = new SimpleMsgFlow<TOut>(pipeCode, option);

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

        #region 枚举器

        /// <summary>
        ///  添加枚举迭代器
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TMsg">消息具体类型</typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static IPipelineMsgEnumerableConnector<TIn, TMsg> Then<TIn, TMsg>(
            this IPipelineConnector<TIn, IEnumerable<TMsg>> pipe, MsgEnumerator<TMsg> nextPipe)
        {
            return pipe.Set(nextPipe);
        }

        /// <summary>
        ///  添加枚举迭代器
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TMsg">消息具体类型</typeparam>
        /// <param name="pipe"></param>
        /// <param name="pipeCode"></param>
        /// <param name="msgFilter"></param>
        /// <returns></returns>
        public static IPipelineMsgEnumerableConnector<TIn, TMsg> ThenWithMsgEnumerator<TIn, TMsg>(
            this IPipelineConnector<TIn, IEnumerable<TMsg>> pipe,
            string pipeCode = null, Func<IEnumerable<TMsg>, IEnumerable<TMsg>> msgFilter = null)

        {
            return pipe.Set(new MsgEnumerator<TMsg>(pipeCode, msgFilter));
        }

        /// <summary>
        ///  添加枚举迭代器
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TMsg">消息具体类型</typeparam>
        /// <typeparam name="TNextOutContext"></typeparam>
        /// <typeparam name="TNextResult"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="iteratorPipe"></param>
        /// <returns></returns>
        public static IPipelineConnector<TIn, IEnumerable<TMsg>> WithIterator<TIn, TMsg, TNextPara, TNextResult,
            TNextOutContext>(this IPipelineMsgEnumerableConnector<TIn, TMsg> pipe,
            BaseFourWayPipe<TMsg, TNextPara, TNextResult, TNextOutContext> iteratorPipe)
        {
            pipe.EndPipe.SetIterator(iteratorPipe);
            return new InterPipelineConnector<TIn, IEnumerable<TMsg>>(pipe.StartPipe, pipe.EndPipe);
        }

        /// <summary>
        ///  添加枚举迭代器
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TMsg">消息具体类型</typeparam>
        /// <typeparam name="TNextOutContext"></typeparam>
        /// <typeparam name="TNextResult"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="iteratorPipe"></param>
        /// <returns></returns>
        public static IPipelineConnector<TIn, IEnumerable<TMsg>> WithIterator<TIn, TMsg, TNextPara, TNextResult,
            TNextOutContext>(this IPipelineMsgEnumerableConnector<TIn, TMsg> pipe,
            BaseOneWayPipe<TMsg> iteratorPipe)
        {
            pipe.EndPipe.SetIterator(iteratorPipe);
            return new InterPipelineConnector<TIn, IEnumerable<TMsg>>(pipe.StartPipe, pipe.EndPipe);
        }

        #endregion
    }
}
