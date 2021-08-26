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


using System;
using System.Threading.Tasks;
using OSS.Pipeline.SimplePipeline.InterImpls.Connector;

namespace OSS.Pipeline
{
    /// <summary>
    ///  pipeline 生成器
    /// </summary>
    public static partial class SimplePipelineBuilder
    {


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
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
        public static ISimplePipelineConnector<TContext> StartWithActivity<TContext>(string pipeCode,
            Func<TContext, Task<TrafficSignal>> exeFunc)
        {
            return Start(new SimpleActivity<TContext>(pipeCode, exeFunc));
        }

        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TNextResult"></typeparam>
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
        public static ISimplePipelineConnector<TContext> StartWithActivity<TContext, TNextResult>(string pipeCode,
            Func<TContext, Task<TrafficSignal<TNextResult>>> exeFunc)
        {
            return Start(new SimpleActivity<TContext, TNextResult>(pipeCode, exeFunc));
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
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
        public static ISimplePipelineConnector<TContext> StartWithEffectActivity<TContext>(string pipeCode,
            Func<TContext, Task<TrafficSignal<TContext>>> exeFunc)
        {
            return Start(new SimpleEffectActivity<TContext, TContext>(pipeCode, exeFunc));
        }
    }
}
