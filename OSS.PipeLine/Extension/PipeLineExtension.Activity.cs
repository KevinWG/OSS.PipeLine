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
using OSS.Pipeline.Interface;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static partial class PipelineExtension
    {
        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc">
        /// 执行委托，返回的处理结果：
        ///     TrafficSignal -（活动是否处理成功，业务结果）
        ///         Green_Pass  - 流体自动流入后续管道
        ///         Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///         Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static IPipelineAppender<TIn, Empty> ThenWithActivity<TIn, TOut>(
            this IPipelineAppender<TIn, TOut> pipe,
            Func<Task<TrafficSignal>> exeFunc, string pipeCode = null)
        {
            return pipe.Then(new SimpleActivity(exeFunc, pipeCode));
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
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
        public static IPipelineAppender<TIn, TOut> ThenWithActivity<TIn, TOut>(this IPipelineAppender<TIn, TOut> pipe,
            Func<TOut, Task<TrafficSignal>> exeFunc, string pipeCode = null)
        {
            return pipe.Then(new SimpleActivity<TOut>(exeFunc, pipeCode));
        }

        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TNextResult"></typeparam>
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
        public static IPipelineAppender<TIn, TOut> ThenWithActivity<TIn, TOut, TNextResult>(this IPipelineAppender<TIn, TOut> pipe,
            Func<TOut, Task<TrafficSignal<TNextResult>>> exeFunc, string pipeCode = null)
        {
            return pipe.Then(new SimpleActivity<TOut, TNextResult>(exeFunc, pipeCode));
        }

        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc">
        /// 执行委托
        /// 结果：
        ///     TrafficSignal &lt;TResult &gt; -（活动是否处理成功，业务结果）
        ///         Green_Pass  - 流体自动流入后续管道
        ///         Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///         Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static IPipelineAppender<TIn, TResult> ThenWithEffectActivity<TIn, TOut, TResult>(
            this IPipelineAppender<TIn, TOut> pipe,
            Func<Task<TrafficSignal<TResult>>> exeFunc, string pipeCode = null)
        {
            return pipe.Then(new SimpleEffectActivity<TResult>(exeFunc, pipeCode));
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TFuncPara"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TIn"></typeparam>
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
        public static IPipelineAppender<TIn, TResult> ThenWithEffectActivity<TIn, TFuncPara, TResult>(
            this IPipelineAppender<TIn, TFuncPara> pipe,
            Func<TFuncPara, Task<TrafficSignal<TResult>>> exeFunc, string pipeCode = null)
        {
            return pipe.Then(new SimpleEffectActivity<TFuncPara, TResult>(exeFunc, pipeCode));
        }


        ///  <summary>
        ///   追加活动管道
        ///  </summary>
        ///  <typeparam name="TFuncPara"></typeparam>
        ///  <typeparam name="TResult"></typeparam>
        ///  <typeparam name="TIn"></typeparam>
        ///  <typeparam name="TOut"></typeparam>
        ///  <param name="pipe"></param>
        ///  <param name="exeFunc">
        /// 执行委托
        ///  参数：
        ///      当前活动上下文信息
        ///  结果：
        ///      TrafficSignal &lt;TResult &gt; -（活动是否处理成功，业务结果）
        ///          Green_Pass  - 流体自动流入后续管道
        ///          Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///          Red_Block - 触发Block，业务流不再向后续管道传递。
        ///  </param>
        ///  <param name="pipeCode"></param>
        ///  <returns></returns>
        public static IPipelineAppender<TIn, TFuncPara> ThenWithFuncActivity<TIn, TOut, TFuncPara, TResult>(
            this IPipelineAppender<TIn, TOut> pipe,
            Func<TFuncPara, Task<TrafficSignal<TResult>>> exeFunc, string pipeCode = null)
        {
            return pipe.Then(new SimpleFuncActivity<TFuncPara, TResult>(exeFunc, pipeCode));
        }


        ///  <summary>
        ///   追加活动管道
        ///  </summary>
        ///  <typeparam name="TFuncPara"></typeparam>
        ///  <typeparam name="TResult"></typeparam>
        ///  <typeparam name="TIn"></typeparam>
        ///  <typeparam name="TOut"></typeparam>
        ///  <param name="pipe"></param>
        ///  <param name="exeFunc">
        /// 执行委托
        ///  参数：当前活动上下文信息
        ///  结果：
        ///      TrafficSignal &lt;TResult &gt; -（活动是否处理成功，业务结果）
        ///          Green_Pass  - 流体自动流入后续管道
        ///          Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///          Red_Block - 触发Block，业务流不再向后续管道传递。
        ///  </param>
        ///  <param name="pipeCode"></param>
        ///  <returns></returns>
        public static IPipelineAppender<TIn, TResult> ThenWithFuncEffectActivity<TIn, TOut, TFuncPara, TResult>(
            this IPipelineAppender<TIn, TOut> pipe,
            Func<TFuncPara, Task<TrafficSignal<TResult>>> exeFunc, string pipeCode = null)
        {
            return pipe.Then(new SimpleFuncEffectActivity<TFuncPara, TResult>(exeFunc, pipeCode));
        }
    }
}