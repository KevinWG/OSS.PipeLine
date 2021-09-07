#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  管道扩展
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System;
using OSS.PipeLine.Gateway.Default;
using OSS.Pipeline.Interface;
using OSS.Pipeline.Pipeline.InterImpls.Connector;
using OSS.Pipeline.Pipeline.InterImpls.Connector.Extension;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static partial class PipelineExtension
    {
    

        /// <summary>
        ///  添加下一个节点
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static IPipelineBranchConnector<TIn, TOut> Then<TIn, TOut>(this IPipelineConnector<TIn, TOut> pipe, BaseBranchGateway<TOut> nextPipe)
        {
            return pipe.Set(nextPipe);
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
        public static IPipelineConnector<TIn, TNextOut> WithBranchBox<TIn, TOut, TNextOut>(this IPipelineBranchConnector<TIn, TOut> pipe, Func<BaseBranchGateway<TOut>, IPipeAppender<TNextOut>> branchGather)
        {
            var newPipe = new InterPipelineConnector<TIn, TNextOut>(pipe.StartPipe, branchGather(pipe.EndBranchPipe));

            pipe.StartPipe     = null;
            pipe.EndBranchPipe = null;

            return newPipe;
        }

        ///// <summary>
        /////  添加下一个节点
        ///// </summary>
        ///// <typeparam name="TIn"></typeparam>
        ///// <typeparam name="TOut"></typeparam>
        ///// <param name="pipe"></param>
        ///// <param name="pipeCode"></param>
        ///// <param name="_conditionFilter"></param>
        ///// <returns></returns>
        //public static IPipelineBranchConnector<TIn, TOut> ThenWithBranch<TIn, TOut>(this IPipelineConnector<TIn, TOut> pipe, string pipeCode = "",
        //    Func<TOut, IPipeMeta, string, bool> _conditionFilter = null)
        //{
        //    return pipe.Set(new SimpleBranchGateway<TOut>(pipeCode, _conditionFilter));
        //}



    }
}