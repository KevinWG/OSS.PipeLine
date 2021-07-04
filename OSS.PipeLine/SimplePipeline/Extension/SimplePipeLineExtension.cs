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
using OSS.Pipeline.SimplePipeline;
using OSS.Pipeline.SimplePipeline.InterImpls.Connector;
using OSS.Pipeline.SimplePipeline.InterImpls.Connector.Extension;

namespace OSS.Pipeline
{
    /// <summary>
    /// EventFlow 创建工厂
    /// </summary>
    public static partial class SimplePipelineExtension
    {
        /// <summary>
        ///  追加下一个节点
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TNextResult"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static ISimplePipelineConnector<TContext> Then<TContext, TNextResult>(this ISimplePipelineConnector<TContext> pipe,
            BaseFourWayPipe<TContext, TContext, TNextResult, TContext> nextPipe)
        {
            return pipe.Set(nextPipe);
        }




        /// <summary>
        ///  添加下一个节点
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static void Then<TContext>(this ISimplePipelineConnector<TContext> pipe, BaseOneWayPipe<TContext> nextPipe)
        {
            pipe.Set(nextPipe);
        }



        #region 分支



        /// <summary>
        ///  添加下一个节点
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static ISimplePipelineBranchConnector<TContext> Then<TContext>(this ISimplePipelineConnector<TContext> pipe, BaseBranchGateway<TContext> nextPipe)
        {
            return pipe.Set(nextPipe);
        }

        /// <summary>
        ///  添加第一个节点
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="branchGather">
        ///     扩展分支方法，并汇聚返回最终节点到主流程
        /// </param>
        /// <returns></returns>
        public static ISimplePipelineConnector<TContext> WithBranchBox<TContext>(this ISimplePipelineBranchConnector<TContext> pipe, Func<BaseBranchGateway<TContext>, IPipeAppender<TContext>> branchGather)
        {
            var newPipe = new InterSimplePipelineConnector<TContext>(pipe.StartPipe, branchGather(pipe.EndBranchPipe));

            pipe.StartPipe     = null;
            pipe.EndBranchPipe = null;

            return newPipe;
        }


        #endregion

        #region 生成Pipeline

        /// <summary>
        ///  根据当前连接信息创建Pipeline
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="pipeCode"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static SimplePipeline<TContext> AsPipeline<TContext>(this ISimplePipelineConnector<TContext> pipe, string pipeCode, PipeLineOption option = null)
        {
            var newPipe = new SimplePipeline<TContext>(pipeCode, pipe.StartPipe, pipe.EndAppender, option);
            pipe.StartPipe   = null;
            pipe.EndAppender = null;
            return newPipe;
        }


        #endregion

    }
}