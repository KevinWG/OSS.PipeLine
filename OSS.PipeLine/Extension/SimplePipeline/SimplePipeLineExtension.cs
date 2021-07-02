//#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

///***************************************************************************
//*　　	文件功能描述：OSS.EventFlow - Pipeline 扩展
//*
//*　　	创建人： Kevin
//*       创建人Email：1985088337@qq.com
//*       创建时间： 2020-11-22
//*       
//*****************************************************************************/

//#endregion

//using System;
//using System.Collections.Generic;
//using OSS.Pipeline.Base;
//using OSS.Pipeline.Base.Base;
//using OSS.Pipeline.Interface;
//using OSS.Pipeline.InterImpls.Pipeline;

//namespace OSS.Pipeline
//{
//    /// <summary>
//    /// EventFlow 创建工厂
//    /// </summary>
//    public static partial class SimplePipeLineExtension
//    {
//        /// <summary>
//        ///  追加下一个节点
//        /// </summary>
//        /// <typeparam name="TIn"></typeparam>
//        /// <typeparam name="TOut"></typeparam>
//        /// <typeparam name="TNextPara"></typeparam>
//        /// <typeparam name="TNextOut"></typeparam>
//        /// <typeparam name="TNextResult"></typeparam>
//        /// <param name="pipe"></param>
//        /// <param name="nextPipe"></param>
//        /// <returns></returns>
//        public static IPipelineAppender<TIn, TNextOut> Then<TIn,  TOut,TNextPara, TNextResult, TNextOut>(this IPipelineAppender<TIn, TOut> pipe,
//            BaseFourWayPipe<TOut, TNextPara, TNextResult, TNextOut> nextPipe)
//        {
//            return pipe.Set(nextPipe);
//        }

//        /// <summary>
//        ///  追加下一个节点
//        /// </summary>
//        /// <typeparam name="TIn"></typeparam>
//        /// <typeparam name="TOut"></typeparam>
//        /// <typeparam name="TNextPara"></typeparam>
//        /// <typeparam name="TNextOut"></typeparam>
//        /// <typeparam name="TNextResult"></typeparam>
//        /// <param name="pipe"></param>
//        /// <param name="nextPipe"></param>
//        /// <returns></returns>
//        public static IPipelineAppender<TIn, TNextOut> Then<TIn, TOut, TNextPara, TNextResult, TNextOut>(this IPipelineAppender<TIn, TOut> pipe,
//            BaseFourWayPipe<Empty, TNextPara, TNextResult, TNextOut> nextPipe)
//        {
//            return pipe.Set(nextPipe);
//        }



//        /// <summary>
//        ///  添加下一个节点
//        /// </summary>
//        /// <typeparam name="TIn"></typeparam>
//        /// <typeparam name="TOut"></typeparam>
//        /// <param name="pipe"></param>
//        /// <param name="nextPipe"></param>
//        /// <returns></returns>
//        public static void Then<TIn, TOut>(this IPipelineAppender<TIn, TOut> pipe, BaseOneWayPipe<TOut> nextPipe)
//        {
//            pipe.Set(nextPipe);
//        }



//        #region 分支



//        /// <summary>
//        ///  添加下一个节点
//        /// </summary>
//        /// <typeparam name="TIn"></typeparam>
//        /// <typeparam name="TOut"></typeparam>
//        /// <param name="pipe"></param>
//        /// <param name="nextPipe"></param>
//        /// <returns></returns>
//        public static IPipelineBranchAppender<TIn, TOut> Then<TIn, TOut>(this IPipelineAppender<TIn, TOut> pipe, BaseBranchGateway<TOut> nextPipe)
//        {
//            return pipe.Set(nextPipe);
//        }

//        /// <summary>
//        ///  添加第一个节点
//        /// </summary>
//        /// <typeparam name="TIn"></typeparam>
//        /// <typeparam name="TOut"></typeparam>
//        /// <typeparam name="TNextOut"></typeparam>
//        /// <param name="pipe"></param>
//        /// <param name="branchGather">
//        ///     扩展分支方法，并汇聚返回最终节点到主流程
//        /// </param>
//        /// <returns></returns>
//        public static IPipelineAppender<TIn, TNextOut> WithBranchBox<TIn, TOut, TNextOut>(this IPipelineBranchAppender<TIn, TOut> pipe, Func<BaseBranchGateway<TOut>, IPipeAppender<TNextOut>> branchGather)
//        {
//            var newPipe = new InterPipelineAppender<TIn, TNextOut>(pipe.StartPipe, branchGather(pipe.EndBranchPipe));

//            pipe.StartPipe     = null;
//            pipe.EndBranchPipe = null;

//            return newPipe;
//        }


//        #endregion


//        #region 枚举器

//        /// <summary>
//        ///  添加下一个节点
//        /// </summary>
//        /// <typeparam name="TIn"></typeparam>
//        /// <typeparam name="TMsg"></typeparam>
//        /// <param name="pipe"></param>
//        /// <param name="nextPipe"></param>
//        /// <returns></returns>
//        public static IPipelineMsgEnumerableAppender<TIn, TMsgEnumerable, TMsg> Then<TIn, TMsgEnumerable, TMsg>(this IPipelineAppender<TIn, TMsgEnumerable> pipe, BaseMsgEnumerator<TMsgEnumerable, TMsg> nextPipe)
//            where TMsgEnumerable : IEnumerable<TMsg>
//        {
//            return pipe.Set(nextPipe);
//        }

//        /// <summary>
//        ///  添加第一个节点
//        /// </summary>
//        /// <typeparam name="TIn"></typeparam>
//        /// <typeparam name="TMsg"></typeparam>
//        /// <typeparam name="TNextOutContext"></typeparam>
//        /// <param name="pipe"></param>
//        /// <param name="iteratorAppender">
//        ///     循环迭代器的迭代管道,枚举器将循环执行此管道
//        /// </param>
//        /// <returns></returns>
//        public static IPipelineAppender<TIn, TMsgEnumerable> WithIterator<TIn, TMsgEnumerable, TMsg, TNextPara, TNextResult, TNextOutContext>(this IPipelineMsgEnumerableAppender<TIn, TMsgEnumerable, TMsg> pipe, 
//            BasePipe<TMsg, TNextPara, TNextResult, TNextOutContext> iteratorPipe)
//            where TMsgEnumerable : IEnumerable<TMsg>
//        {
//            pipe.EndPipe.SetIterator(iteratorPipe);
//            return new InterPipelineAppender<TIn, TMsgEnumerable>(pipe.StartPipe,pipe.EndPipe);
//        }

//        #endregion

        
//        internal static IPipelineAppender<TIn, TNextOut> Set<TIn, TOut, TNextPara, TNextResult, TNextOut>(this IPipelineAppender<TIn, TOut> oldAppender,
//            BaseFourWayPipe<TOut, TNextPara, TNextResult, TNextOut> endPipe)
//        {
//            oldAppender.EndAppender.Append(endPipe);
//            IPipelineAppender<TIn, TNextOut> pipelineAppender = new InterPipelineAppender<TIn, TNextOut>(oldAppender.StartPipe, endPipe);

//            oldAppender.StartPipe   = null;
//            oldAppender.EndAppender = null;
//            return pipelineAppender;
//        }


//        internal static IPipelineAppender<TIn, TNextOut> Set<TIn, TOut, TNextPara, TNextResult, TNextOut>(this IPipelineAppender<TIn, TOut> oldAppender,
//            BaseFourWayPipe<Empty, TNextPara, TNextResult, TNextOut> endPipe)
//        {
//            oldAppender.EndAppender.Append(endPipe);
//            IPipelineAppender<TIn, TNextOut> appender =
//                new InterPipelineAppender<TIn, TNextOut>(oldAppender.StartPipe, endPipe);

//            oldAppender.StartPipe   = null;
//            oldAppender.EndAppender = null;
//            return appender;
//        }

//        internal static void Set<TIn, TOut>(this IPipelineAppender<TIn, TOut> oldAppender,
//            BaseOneWayPipe<TOut> endPipe)
//        {
//            oldAppender.EndAppender.Append(endPipe);

//            oldAppender.StartPipe   = null;
//            oldAppender.EndAppender = null;
//        }


//        internal static IPipelineBranchAppender<TIn, TOut> Set<TIn, TOut>(this IPipelineAppender<TIn, TOut> oldAppender,
//            BaseBranchGateway<TOut> endPipe)
//        {
//            oldAppender.EndAppender.Append(endPipe);
//            IPipelineBranchAppender<TIn, TOut> appender =
//                new InterPipelineBranchAppender<TIn, TOut>(oldAppender.StartPipe, endPipe);

//            return appender;
//        }


//        internal static IPipelineMsgEnumerableAppender<TIn, TMsgEnumerable, TMsg> Set<TIn, TMsgEnumerable, TMsg>(
//            this IPipelineAppender<TIn, TMsgEnumerable> oldAppender,
//            BaseMsgEnumerator<TMsgEnumerable, TMsg> endPipe)
//            where TMsgEnumerable : IEnumerable<TMsg>
//        {
//            oldAppender.EndAppender.Append(endPipe);

//            var appender =
//                new InterPipelineMsgEnumerableAppender<TIn, TMsgEnumerable, TMsg>(oldAppender.StartPipe, endPipe);

//            return appender;
//        }
//    }
//}