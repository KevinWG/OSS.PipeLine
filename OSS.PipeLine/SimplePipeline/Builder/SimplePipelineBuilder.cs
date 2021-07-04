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
using OSS.Pipeline.SimplePipeline.InterImpls.Connector;

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
        public static ISimplePipelineConnector<TContext> Start<TContext, TResult>(BaseFourWayPipe<TContext, TContext, TResult, TContext> startPipe)
        {
            return new InterSimplePipelineConnector<TContext>( startPipe, startPipe);
        }
        
        /// <summary>
        ///  添加第一个节点
        /// </summary>
        /// <param name="startPipe"></param>
        /// <returns></returns>
        public static ISimplePipelineBranchConnector<TContext> Start<TContext>(BaseBranchGateway<TContext> startPipe)
        {
            return new InterSimplePipelineBranchConnector<TContext>( startPipe, startPipe);
        }

    }
}
