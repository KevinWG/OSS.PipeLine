#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  流体扩展
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System;
using System.Threading.Tasks;
using OSS.Pipeline.SimplePipeline.InterImpls.Connector;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static partial class SimplePipelineExtension
    {
     

        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <param name="pipe"></param>
        /// <param name="exeFunc">
        /// 执行委托
        /// 参数：当前活动上下文（会继续传递给下一个节点）
        /// 结果：
        ///     TrafficSignal -（活动是否处理成功，业务结果）
        ///         Green_Pass  - 流体自动流入后续管道
        ///         Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///         Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static ISimplePipelineConnector<TContext> ThenWithActivity<TContext>(this ISimplePipelineConnector<TContext> pipe, string pipeCode,
            Func<TContext, Task<TrafficSignal>> exeFunc)
        {
            return pipe.Then(new SimpleActivity<TContext>(pipeCode,exeFunc));
        }

        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <param name="pipe"></param>
        /// <param name="exeFunc">
        /// 执行委托
        /// 参数：当前活动上下文（会继续传递给下一个节点）
        /// 结果：
        ///     TrafficSignal -（活动是否处理成功，业务结果）
        ///         Green_Pass  - 流体自动流入后续管道
        ///         Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///         Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static ISimplePipelineConnector<TContext> ThenWithActivity<TContext, TNextResult>(this ISimplePipelineConnector<TContext> pipe, string pipeCode,
            Func<TContext, Task<TrafficSignal<TNextResult>>> exeFunc)
        {
            return pipe.Then(new SimpleActivity<TContext, TNextResult>(pipeCode,exeFunc));
        }
        
        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <param name="pipe"></param>
        /// <param name="exeFunc">
        /// 执行委托
        /// 参数：
        ///     当前活动上下文信息
        /// 结果：
        ///     TrafficSignal &lt;TResult &gt; -（活动是否处理成功，业务结果）
        ///         Green_Pass  - 流体自动流入后续管道
        ///         Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///         Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static ISimplePipelineConnector<TContext> ThenWithEffectActivity<TContext>(
            this ISimplePipelineConnector<TContext> pipe, string pipeCode,
            Func<TContext, Task<TrafficSignal<TContext>>> exeFunc)
        {
            return pipe.Then(new SimpleEffectActivity<TContext, TContext>(pipeCode,exeFunc));
        }

    }
}