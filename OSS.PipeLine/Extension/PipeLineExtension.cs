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
            this BaseInPipePart<InFlowContext> startPipe, IOutPipeAppender<OutFlowContext> endPipeAppender,
            string flowPipeCode = null,PipeLineOption option=null)
        {
            var code = flowPipeCode ?? string.Concat(startPipe.PipeCode, "-", endPipeAppender.PipeCode);

            return new Pipeline<InFlowContext, OutFlowContext>(startPipe, endPipeAppender,option) {PipeCode = code};
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
            return PipelineFactory.Set(new InterPipelineAppender<TIn, TNextOut>(), pipe.StartPipe, nextPipe);
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
            return PipelineFactory.Set(new InterPipelineAppender<TIn, TNextOut>(), pipe.StartPipe, nextPipe);
        }


        /// <summary>
        ///  添加第一个节点
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static IPipelineBranchAppender<TIn, TOut> Then<TIn, TOut>(this IPipelineAppender<TIn, TOut> pipe, BaseBranchGateway<TOut> nextPipe)
        {
            return PipelineFactory.Set(new InterPipelineBranchAppender<TIn, TOut>(), pipe.StartPipe, nextPipe);
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
        public static IPipelineAppender<TIn, TNextOut> WithBranchBox<TIn,TOut, TNextOut>(this IPipelineBranchAppender<TIn,TOut> pipe, Func<BaseBranchGateway<TOut>,IOutPipeAppender<TNextOut>> branchGather)
        {
            return PipelineFactory.Set(new InterPipelineAppender<TIn, TNextOut>(), pipe.StartPipe, branchGather(pipe.EndAppender));
        }



    }
}