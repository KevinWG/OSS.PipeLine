#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  管道扩展-消息类
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
    /// 管道扩展类
    /// </summary>
    public static partial class PipeExtension
    {
        /// <summary>
        ///  追加默认消息发布者管道
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="pipeCode">消息pipeDataKey，默认对应的flow是异步线程池</param>
        /// <param name="pushKeyGenerator">消息key生成器,为空则使用pipeCode作为发布消息key</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static void AppendMsgPublisher<TMsg>(this IPipeAppender<TMsg> pipe, string pipeCode,
            Func<TMsg, string> pushKeyGenerator = null, DataPublisherOption option = null)
        {
            var nextPipe = new MsgPublisher<TMsg>(pipeCode,pushKeyGenerator, option);
            pipe.InterAppend(nextPipe);
        }

        /// <summary>
        ///  追加默认消息订阅者管道
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="pipeCode">消息pipeDataKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static BaseMsgSubscriber<TMsg> AppendMsgSubscriber<TMsg>(this IPipeAppender<TMsg> pipe, string pipeCode,
            DataFlowOption option = null)
        {
            var nextPipe = new MsgSubscriber<TMsg>(pipeCode, option);

            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        ///  追加默认消息流管道
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="pipeCode">消息pipeDataKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static BaseMsgFlow<TMsg> AppendMsgFlow<TMsg>(this IPipeAppender<TMsg> pipe, string pipeCode,
            DataFlowOption option = null)
        {
            var nextPipe = new MsgFlow<TMsg>(pipeCode, option);

            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加默认消息转换管道
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="convertFunc"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseMsgConverter<TMsg, NextOutContext> AppendMsgConverter<TMsg, NextOutContext>(
            this IPipeAppender<TMsg> pipe,  Func<TMsg, NextOutContext> convertFunc, string pipeCode=null)
        {
            var nextPipe = new InterMsgConvertor<TMsg, NextOutContext>(pipeCode, convertFunc);
         
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加消息迭代器
        /// </summary>
        /// <typeparam name="TMsg">消息具体类型</typeparam>
        /// <typeparam name="TMsgEnumerable">消息的枚举类型如 IList&lt;TMsg&gt;</typeparam>
        /// <param name="pipe"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseMsgEnumerator<TMsgEnumerable,TMsg> AppendMsgEnumerator<TMsgEnumerable, TMsg>(this IPipeAppender<TMsgEnumerable> pipe, string pipeCode=null)
            where TMsgEnumerable : IEnumerable<TMsg>
        {
            var nextPipe = new BaseMsgEnumerator<TMsgEnumerable,TMsg>(pipeCode);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }
    }
}