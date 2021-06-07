using System;
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
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="msgFlowKey">消息flowKey，默认对应的flow是异步线程池</param>
        /// <param name="pipeCode"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static void AppendMsgPublisher<OutContext>(this IOutPipeAppender<OutContext> pipe, string msgFlowKey, string pipeCode = null, DataPublisherOption option=null)
        {
            var nextPipe = new InterMsgPublisher<OutContext>(msgFlowKey, option);
            if (!string.IsNullOrEmpty(pipeCode))
            {
                nextPipe.PipeCode = pipeCode;
            }
            pipe.InterAppend(nextPipe);
        }

        /// <summary>
        ///  追加默认消息订阅者管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="msgFlowKey">消息flowKey，默认对应的flow是异步线程池</param>
        /// <param name="pipeCode"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static BaseMsgSubscriber<OutContext> AppendMsgSubscriber<OutContext>(this IOutPipeAppender<OutContext> pipe, string msgFlowKey, string pipeCode = null, DataFlowOption option=null)
        {
            var nextPipe = new InterMsgSubscriber<OutContext>(msgFlowKey, option);
            if (!string.IsNullOrEmpty(pipeCode))
            {
                nextPipe.PipeCode = pipeCode;
            }
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        ///  追加默认消息流管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="msgFlowKey">消息flowKey，默认对应的flow是异步线程池</param>
        /// <param name="pipeCode"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static BaseMsgFlow<OutContext> AppendMsgFlow<OutContext>(this IOutPipeAppender<OutContext> pipe, string msgFlowKey , string pipeCode = null, DataFlowOption option=null)
        {
            var nextPipe = new InterMsgFlow<OutContext>(msgFlowKey, option);
            if (!string.IsNullOrEmpty(pipeCode))
            {
                nextPipe.PipeCode = pipeCode;
            }
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加默认消息转换管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="convertFunc"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseMsgConverter<OutContext, NextOutContext> AppendMsgConverter<OutContext, NextOutContext>(
            this IOutPipeAppender<OutContext> pipe, Func<OutContext, NextOutContext> convertFunc, string pipeCode = null)
        {
            var nextPipe = new InterMsgConvertor<OutContext, NextOutContext>(convertFunc);
            if (!string.IsNullOrEmpty(pipeCode))
            {
                nextPipe.PipeCode = pipeCode;
            }
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

       
    }
}