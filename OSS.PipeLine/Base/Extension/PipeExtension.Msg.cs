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

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static partial class PipeExtension
    {
        /// <summary>
        /// 追加枚举器
        /// </summary>
        /// <typeparam name="TMsg">消息具体类型</typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static MsgEnumerator<TMsg> Append<TMsg>(this IPipeAppender<IEnumerable<TMsg>> pipe, MsgEnumerator<TMsg> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }
        
        /// <summary>
        ///  追加默认消息发布者管道
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="msgDataKey">消息pipeDataKey，默认消息实现对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static void AppendMsgPublisher<TMsg>(this IPipeAppender<TMsg> pipe, string msgDataKey, DataPublisherOption option = null, string pipeCode = null)
        {
            var nextPipe = new SimpleMsgPublisher<TMsg>(msgDataKey, option, pipeCode);
            pipe.InterAppend(nextPipe);
        }

        /// <summary>
        ///  追加默认消息发布者管道
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="pushKeyGenerator">消息key生成器,为空则使用pipeCode作为发布消息key</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static void AppendMsgPublisher<TMsg>(this IPipeAppender<TMsg> pipe, Func<TMsg, string> pushKeyGenerator, DataPublisherOption option = null, string pipeCode = null)
        {
            var nextPipe = new SimpleMsgPublisher<TMsg>( pushKeyGenerator, option,pipeCode);
            pipe.InterAppend(nextPipe);
        }

        /// <summary>
        ///  追加默认消息订阅者管道
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="msgDataKey">消息pipeDataKey，默认对应的flow是异步线程池</param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseMsgSubscriber<TMsg> AppendMsgSubscriber<TMsg>(this IPipeAppender<TMsg> pipe, string msgDataKey, string pipeCode = null)
        {
            var nextPipe = new SimpleMsgSubscriber<TMsg>(msgDataKey,pipeCode);

            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加默认消息流管道
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="msgDataKey">消息pipeDataKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseMsgFlow<TMsg> AppendMsgFlow<TMsg>(this IPipeAppender<TMsg> pipe, string msgDataKey,
                                                            DataFlowOption option = null, string pipeCode = null)
        {
            var nextPipe = new SimpleMsgFlow<TMsg>(msgDataKey, option,pipeCode);

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
            this IPipeAppender<TMsg> pipe, Func<TMsg, NextOutContext> convertFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleMsgConvertor<TMsg, NextOutContext>( convertFunc, pipeCode);

            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加消息迭代器
        /// </summary>
        /// <typeparam name="TMsg">消息具体类型</typeparam>
        /// <param name="pipe"></param>
        /// <param name="pipeCode"></param>
        /// <param name="msgFilter">消息过滤器</param>
        /// <returns></returns>
        public static MsgEnumerator<TMsg> AppendMsgEnumerator<TMsg>(this IPipeAppender<IEnumerable<TMsg>> pipe, Func<IEnumerable<TMsg>, IEnumerable<TMsg>> msgFilter = null, string pipeCode = null)
        {
            var nextPipe = new MsgEnumerator<TMsg>( msgFilter, pipeCode);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }
    }
}