#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  流体生成器
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;
using OSS.Pipeline.InterImpls.Pipeline;
using System.Collections.Generic;

namespace OSS.Pipeline
{
    /// <summary>
    ///  pipeline 生成器
    /// </summary>
    public static partial class SimplePipelineBuilder
    {
        /// <summary>
        ///  添加第一个节点
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="startPipe"></param>
        /// <returns></returns>
        public static ISimplePipelineAppender<TContext> Start<TContext,  TResult>(BaseThreeWayPipe<TContext, TResult, TContext> startPipe)
        {
            return new InterSimplePipelineAppender<TContext>(startPipe, startPipe);
        }

        ///// <summary>
        /////  添加第一个节点
        ///// </summary>
        ///// <typeparam name="TPara"></typeparam>
        ///// <typeparam name="TOut"></typeparam>
        ///// <typeparam name="TResult"></typeparam>
        ///// <param name="startPipe"></param>
        ///// <returns></returns>
        //public static ISimplePipelineAppender<TContext> Start<TContext, TResult>(BaseThreeWayPassivePipe<Empty, TContext, TResult, TContext> startPipe)
        //{
        //    return new InterPipelineAppender<Empty, TOut>(startPipe, startPipe);
        //}

        ///// <summary>
        /////  添加第一个节点
        ///// </summary>
        ///// <typeparam name="TIn"></typeparam>
        ///// <param name="startPipe"></param>
        ///// <returns></returns>
        //public static ISimplePipelineBranchAppender<TIn, TIn> Start<TIn>(BaseBranchGateway<TIn> startPipe)
        //{
        //    return new InterPipelineBranchAppender<TIn, TIn>(startPipe, startPipe);
        //}


        ///// <summary>
        ///// 根据首位两个管道建立流体
        ///// </summary>
        ///// <typeparam name="InFlowContext"></typeparam>
        ///// <typeparam name="OutFlowContext"></typeparam>
        ///// <param name="startPipe"></param>
        ///// <param name="endPipeAppender"></param>
        ///// <param name="flowPipeCode"></param>
        ///// <param name="option"></param>
        ///// <returns></returns>
        //public static Pipeline<InFlowContext, OutFlowContext> AsPipelineStartAndEndWith<InFlowContext, OutFlowContext>(this BaseInPipePart<InFlowContext> startPipe, ISimplePipeAppender<OutFlowContext> endPipeAppender,
        //    string flowPipeCode, PipeLineOption option = null)
        //{
        //    return new Pipeline<InFlowContext, OutFlowContext>(flowPipeCode, startPipe, endPipeAppender, option);
        //}

        ///// <summary>
        /////  追加下一个节点
        ///// </summary>
        ///// <typeparam name="TIn"></typeparam>
        ///// <typeparam name="TOut"></typeparam>
        ///// <param name="pipe"></param>
        ///// <param name="pipeCode"></param>
        ///// <param name="option"></param>
        ///// <returns></returns>
        //public static Pipeline<TIn, TOut> AsPipeline<TIn, TOut>(this ISimplePipelineAppender<TIn, TOut> pipe, string pipeCode, PipeLineOption option = null)
        //{
        //    var newPipe = new Pipeline<TIn, TOut>(pipeCode, pipe.StartPipe, pipe.EndAppender, option);
        //    pipe.StartPipe = null;
        //    pipe.EndAppender = null;
        //    return newPipe;
        //}

    }
}
