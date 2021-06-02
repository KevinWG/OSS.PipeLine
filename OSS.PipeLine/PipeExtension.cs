﻿using System;
using OSS.Pipeline.Gateway;
using OSS.Pipeline.Interface;
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
        ///  追加 数据转换管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="convertFunc"></param>
        /// <returns></returns>
        public static BaseMsgConvertor<OutContext, NextOutContext> Append<OutContext, NextOutContext>(
            this IOutPipeAppender<OutContext> pipe, Func<OutContext, NextOutContext> convertFunc)
        {
            var connector = new DefaultMsgConvertor<OutContext, NextOutContext>(convertFunc);
            pipe.InterAppend(connector);
            return connector;
        }

        /// <summary>
        ///  追加异步流缓冲组件
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="msgFlowKey">消息flowKey，默认对应的flow是异步线程池</param>
        /// <returns></returns>
        public static MsgFlow<OutContext> AppendMsgFlow<OutContext>(this IOutPipeAppender<OutContext> pipe, string msgFlowKey = null)
        {
            var connector = new MsgFlow<OutContext>(msgFlowKey);
            pipe.InterAppend(connector);
            return connector;
        }

    }
}