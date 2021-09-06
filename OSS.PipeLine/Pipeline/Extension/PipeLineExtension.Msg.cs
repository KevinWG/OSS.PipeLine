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
        /// <typeparam name="TMsgEnumerable">消息的枚举类型如 IList&lt;TMsg&gt;</typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static IPipelineMsgEnumerableConnector<TIn, TMsgEnumerable, TMsg> Then<TIn, TMsgEnumerable, TMsg>(this IPipelineConnector<TIn, TMsgEnumerable> pipe, MsgEnumerator<TMsgEnumerable, TMsg> nextPipe)
            where TMsgEnumerable : IEnumerable<TMsg>
        {
            return pipe.Set(nextPipe);
        }

        /// <summary>
        ///  添加枚举迭代器
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TMsg">消息具体类型</typeparam>
        /// <typeparam name="TMsgEnumerable">消息的枚举类型如 IList&lt;TMsg&gt;</typeparam>
        /// <param name="pipe"></param>
        /// <param name="pipeCode"></param>
        /// <param name="msgFilter"></param>
        /// <returns></returns>
        public static IPipelineMsgEnumerableConnector<TIn, TMsgEnumerable, TMsg> ThenWithMsgEnumerator<TIn, TMsgEnumerable, TMsg>(this IPipelineConnector<TIn, TMsgEnumerable> pipe,
            string pipeCode = null, Func<TMsgEnumerable, TMsgEnumerable> msgFilter = null)
            where TMsgEnumerable : IEnumerable<TMsg>
        {
            return pipe.Set(new MsgEnumerator<TMsgEnumerable, TMsg>(pipeCode, msgFilter));
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
        public static IPipelineMsgEnumerableConnector<TIn, IList<TMsg>, TMsg> ThenWithMsgList<TIn, TMsg>(this IPipelineConnector<TIn, IList<TMsg>> pipe,
            string pipeCode = null, Func<IList<TMsg>, IList<TMsg>> msgFilter = null)
        {
            return pipe.Set(new MsgEnumerator<IList<TMsg>, TMsg>(pipeCode, msgFilter));
        }


        /// <summary>
        ///  添加枚举迭代器
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TMsg">消息具体类型</typeparam>
        /// <typeparam name="TMsgEnumerable">消息的枚举类型如 IList&lt;TMsg&gt;</typeparam>
        /// <typeparam name="TNextOutContext"></typeparam>
        /// <typeparam name="TNextResult"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="iteratorPipe"></param>
        /// <returns></returns>
        public static IPipelineConnector<TIn, TMsgEnumerable> WithIterator<TIn, TMsgEnumerable, TMsg, TNextPara, TNextResult, TNextOutContext>(this IPipelineMsgEnumerableConnector<TIn, TMsgEnumerable, TMsg> pipe,
            BaseFourWayPipe<TMsg, TNextPara, TNextResult, TNextOutContext> iteratorPipe)
            where TMsgEnumerable : IEnumerable<TMsg>
        {
            pipe.EndPipe.SetIterator(iteratorPipe);
            return new InterPipelineConnector<TIn, TMsgEnumerable>(pipe.StartPipe, pipe.EndPipe);
        }

        /// <summary>
        ///  添加枚举迭代器
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TMsg">消息具体类型</typeparam>
        /// <typeparam name="TMsgEnumerable">消息的枚举类型如 IList&lt;TMsg&gt;</typeparam>
        /// <typeparam name="TNextOutContext"></typeparam>
        /// <typeparam name="TNextResult"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="iteratorPipe"></param>
        /// <returns></returns>
        public static IPipelineConnector<TIn, TMsgEnumerable> WithIterator<TIn, TMsgEnumerable, TMsg, TNextPara, TNextResult, TNextOutContext>(this IPipelineMsgEnumerableConnector<TIn, TMsgEnumerable, TMsg> pipe,
            BaseOneWayPipe<TMsg> iteratorPipe)
            where TMsgEnumerable : IEnumerable<TMsg>
        {
            pipe.EndPipe.SetIterator(iteratorPipe);
            return new InterPipelineConnector<TIn, TMsgEnumerable>(pipe.StartPipe, pipe.EndPipe);
        }
        
        #endregion



    }
}
