#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - Pipeline 扩展
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System;
using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;
using OSS.Pipeline.InterImpls.Pipeline;

namespace OSS.Pipeline
{
    /// <summary>
    /// EventFlow 创建工厂
    /// </summary>
    public static class PipelineExtension
    {

        /// <summary>
        /// 根据首位两个管道建立流体
        /// </summary>
        /// <typeparam name="InFlowContext"></typeparam>
        /// <typeparam name="OutFlowContext"></typeparam>
        /// <param name="startPipe"></param>
        /// <param name="endPipeAppender"></param>
        /// <param name="flowPipeCode"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static Pipeline<InFlowContext, OutFlowContext> AsFlowStartAndEndWith<InFlowContext, OutFlowContext>(
            this BaseInPipePart<InFlowContext> startPipe, IPipeAppender<OutFlowContext> endPipeAppender,
            string flowPipeCode, PipeLineOption option = null)
        {
            return new Pipeline<InFlowContext, OutFlowContext>(flowPipeCode, startPipe, endPipeAppender, option);
        }


        /// <summary>
        ///  追加下一个节点
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <typeparam name="TNextOut"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static IPipelineAppender<TIn, TNextOut> Then<TIn,  TOut,TNextPara, TNextOut>(this IPipelineAppender<TIn, TOut> pipe,
            BasePipe<TOut, TNextPara, TNextOut> nextPipe)
        {
            var  newPipe= PipelineFactory.Set(new InterPipelineAppender<TIn, TNextOut>(), pipe.StartPipe, nextPipe);
            pipe.StartPipe   = null;
            pipe.EndAppender = null;
            return newPipe;
        }

        /// <summary>
        ///  追加下一个节点
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <typeparam name="TNextOut"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static IPipelineAppender<TIn, TNextOut> Then<TIn, TOut, TNextPara, TNextOut>(this IPipelineAppender<TIn, TOut> pipe,
            BasePipe<EmptyContext, TNextPara, TNextOut> nextPipe)
        {
            var newPipe = PipelineFactory.Set(new InterPipelineAppender<TIn, TNextOut>(), pipe.StartPipe, nextPipe);
            pipe.StartPipe   = null;
            pipe.EndAppender = null;
            return newPipe;
        }


        /// <summary>
        ///  追加下一个节点
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="pipeCode"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static Pipeline<TIn, TOut> AsPipeline<TIn, TOut>(this IPipelineAppender<TIn, TOut> pipe, string pipeCode, PipeLineOption option = null)
        {
            var newPipe = new Pipeline<TIn, TOut>(pipeCode,pipe.StartPipe,pipe.EndAppender, option);
            pipe.StartPipe   = null;
            pipe.EndAppender = null;
            return newPipe;
        }


        /// <summary>
        ///  添加下一个节点
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static IPipelineBranchAppender<TIn, TOut> Then<TIn, TOut>(this IPipelineAppender<TIn, TOut> pipe, BaseBranchGateway<TOut> nextPipe)
        {
            var newPipe = PipelineFactory.Set(new InterPipelineBranchAppender<TIn, TOut>(), pipe.StartPipe, nextPipe);
            pipe.StartPipe   = null;
            pipe.EndAppender = null;
            return newPipe;
        }


        /// <summary>
        ///  添加第一个节点
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TNextOut"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="branchGather">
        ///     扩展分支方法，并汇聚返回最终节点到主流程
        /// </param>
        /// <returns></returns>
        public static IPipelineAppender<TIn, TNextOut> WithBranchBox<TIn,TOut, TNextOut>(this IPipelineBranchAppender<TIn,TOut> pipe, Func<BaseBranchGateway<TOut>,IPipeAppender<TNextOut>> branchGather)
        {
            var newPipe = PipelineFactory.Set(new InterPipelineAppender<TIn, TNextOut>(), pipe.StartPipe, branchGather(pipe.EndAppender));
            pipe.StartPipe   = null;
            pipe.EndAppender = null;
            return newPipe;
        }



    }
}