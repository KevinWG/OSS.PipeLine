//#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

///***************************************************************************
//*　　	文件功能描述：OSS.EventFlow -  流体扩展
//*
//*　　	创建人： Kevin
//*       创建人Email：1985088337@qq.com
//*       创建时间： 2020-11-22
//*       
//*****************************************************************************/

//#endregion


//using System;
//using System.Collections.Generic;
//using OSS.DataFlow;
//using OSS.Pipeline.Interface;
//using OSS.Pipeline.InterImpls.Msg;

//namespace OSS.Pipeline
//{
//    /// <summary>
//    ///  pipeline 生成器
//    /// </summary>
//    public static partial class SimplePipeLineExtension
//    {
//        /// <summary>
//        ///  追加默认消息订阅者管道
//        /// </summary>
//        /// <typeparam name="TOut"></typeparam>
//        /// <typeparam name="TIn"></typeparam>
//        /// <param name="pipe"></param>
//        /// <param name="pipeCode">消息pipeDataKey，默认对应的flow是异步线程池</param>
//        /// <param name="option"></param>
//        /// <returns></returns>
//        public static IPipelineAppender<TIn, TOut> ThenWithMsgSubscriber<TIn, TOut>(
//            this IPipelineAppender<TIn, TOut> pipe,
//            string pipeCode, DataFlowOption option = null)
//        {
//            return pipe.Then(new MsgSubscriber<TOut>(pipeCode, option));
//        }


//        /// <summary>
//        ///  追加默认消息流管道
//        /// </summary>
//        /// <param name="pipe"></param>
//        /// <param name="pipeCode">消息pipeDataKey，默认对应的flow是异步线程池</param>
//        /// <param name="option"></param>
//        /// <returns></returns>
//        public static IPipelineAppender<TIn, TOut> ThenWithMsgFlow<TIn, TOut>(
//            this IPipelineAppender<TIn, TOut> pipe, 
//            string pipeCode, DataFlowOption option = null)
//        {
//            var nextPipe = new MsgFlow<TOut>(pipeCode, option);

//            return pipe.Then(nextPipe);
//        }

//        /// <summary>
//        ///  追加默认消息转换管道
//        /// </summary>
//        /// <param name="pipe"></param>
//        /// <param name="convertFunc"></param>
//        /// <param name="pipeCode"></param>
//        /// <returns></returns>
//        public static IPipelineAppender<TIn, TNextOut> ThenWithMsgConverter<TIn, TOut, TNextOut>(
//            this IPipelineAppender<TIn, TOut> pipe, 
//            Func<TOut, TNextOut> convertFunc, string pipeCode = null)
//        {
//            var nextPipe = new InterMsgConvertor<TOut, TNextOut>(pipeCode, convertFunc);
//            return pipe.Then(nextPipe);
//        }


//        /// <summary>
//        ///  追加消息枚举器
//        /// </summary>
//        /// <param name="pipe"></param>
//        /// <param name="pipeCode"></param>
//        /// <returns></returns>
//        public static IPipelineMsgEnumerableAppender<TIn, TMsgEnumerable, TMsg> ThenWithMsgEnumerator<TIn, TMsgEnumerable, TMsg>(
//            this IPipelineAppender<TIn, TMsgEnumerable> pipe, string pipeCode=null)
//            where TMsgEnumerable : IEnumerable<TMsg>
//        {
//            var nextPipe = new BaseMsgEnumerator<TMsgEnumerable, TMsg>(pipeCode);
//            return pipe.Then(nextPipe);
//        }
//    }
//}
