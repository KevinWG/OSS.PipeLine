using System;
using OSS.Pipeline.Gateway;
using OSS.Pipeline.Interface;
using OSS.Pipeline.InterImpls.Msg;
using OSS.Pipeline.Msg;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static class PipeExtension
    {
        /// <summary>
        ///  追加管道
        /// </summary>
        /// <typeparam name="TOutContext"></typeparam>
        /// <typeparam name="TNextOutContext"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static BasePipe<EmptyContext, TNextPara, TNextOutContext> Append<TOutContext, TNextPara, TNextOutContext>(
            this IOutPipeAppender<TOutContext> pipe, BasePipe<EmptyContext, TNextPara, TNextOutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加管道
        /// </summary>
        /// <typeparam name="TOutContext"></typeparam>
        /// <typeparam name="TNextOutContext"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static BasePipe<TOutContext, TNextPara, TNextOutContext> Append<TOutContext, TNextPara,TNextOutContext>(
            this IOutPipeAppender<TOutContext> pipe, BasePipe<TOutContext,TNextPara, TNextOutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }
        
        /// <summary>
        /// 追加分支网关管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static BaseBranchGateway<OutContext> Append<OutContext>(
            this IOutPipeAppender<OutContext> pipe, BaseBranchGateway<OutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        /// 追加消息发布者管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static void Append<OutContext>(this IOutPipeAppender<OutContext> pipe, BaseMsgPublisher<OutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
        }

        /// <summary>
        ///  追加消息发布者管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="msgFlowKey">消息flowKey，默认对应的flow是异步线程池</param>
        /// <returns></returns>
        public static void AppendMsgPublisher<OutContext>(this IOutPipeAppender<OutContext> pipe, string msgFlowKey = null)
        {
            var nextPipe = new InterMsgPublisher<OutContext>(msgFlowKey);
            pipe.InterAppend(nextPipe);
        }

        /// <summary>
        ///  追加消息发布者管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="msgFlowKey">消息flowKey，默认对应的flow是异步线程池</param>
        /// <returns></returns>
        public static BaseMsgSubscriber<OutContext> AppendMsgSubscriber<OutContext>(this IOutPipeAppender<OutContext> pipe, string msgFlowKey = null)
        {
            var nextPipe = new InterMsgSubscriber<OutContext>(msgFlowKey);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        ///  追加消息流管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="msgFlowKey">消息flowKey，默认对应的flow是异步线程池</param>
        /// <returns></returns>
        public static BaseMsgFlow<OutContext> AppendMsgFlow<OutContext>(this IOutPipeAppender<OutContext> pipe, string msgFlowKey = null)
        {
            var nextPipe = new InterMsgFlow<OutContext>(msgFlowKey);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加消息转换管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="convertFunc"></param>
        /// <returns></returns>
        public static BaseMsgConverter<OutContext, NextOutContext> AppendMsgConverter<OutContext, NextOutContext>(
            this IOutPipeAppender<OutContext> pipe, Func<OutContext, NextOutContext> convertFunc)
        {
            var nextPipe = new InterMsgConvertor<OutContext, NextOutContext>(convertFunc);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

       
    }
}