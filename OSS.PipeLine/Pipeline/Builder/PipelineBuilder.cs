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
using OSS.Pipeline.Pipeline.InterImpls.Connector;

namespace OSS.Pipeline
{
    /// <summary>
    ///  pipeline 生成器
    /// </summary>
    public static partial class PipelineBuilder
    {
        /// <summary>
        ///  添加第一个节点
        /// </summary>
        /// <typeparam name="TPara"></typeparam>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="startPipe"></param>
        /// <returns></returns>
        public static IPipelineConnector<TIn, TOut> Start<TIn, TPara, TResult, TOut>(BaseFourWayPipe<TIn, TPara,TResult, TOut> startPipe)
        {
            return new InterPipelineConnector<TIn, TOut>( startPipe, startPipe);
        }

        /// <summary>
        ///  添加第一个节点
        /// </summary>
        /// <typeparam name="TPara"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="startPipe"></param>
        /// <returns></returns>
        public static IPipelineConnector<Empty, TOut> Start< TPara, TResult, TOut>(BaseFourWayPipe<Empty, TPara, TResult, TOut> startPipe)
        {
            return new InterPipelineConnector<Empty, TOut>( startPipe, startPipe);
        }
        
        /// <summary>
        ///  添加第一个节点
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="startPipe"></param>
        /// <returns></returns>
        public static IPipelineBranchConnector<TIn, TIn> Start<TIn>(BaseBranchGateway<TIn> startPipe)
        {
            return new InterPipelineBranchConnector<TIn, TIn>( startPipe, startPipe);
        }
    }
}
