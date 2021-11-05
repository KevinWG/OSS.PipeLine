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
using OSS.Pipeline.Pipeline.InterImpls.Connector;

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
        public static IPipelineConnector<TIn, Empty> ThenWithActivity<TIn, TOut>(
            this IPipelineConnector<TIn, TOut> pipe, string pipeCode,
            Func<Task<TrafficSignal>> exeFunc)
        {
            return pipe.Then(new SimpleActivity(pipeCode, exeFunc));
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
        public static IPipelineConnector<TIn, TOut> ThenWithActivity<TIn, TOut>(this IPipelineConnector<TIn, TOut> pipe, string pipeCode,
            Func<TOut, Task<TrafficSignal>> exeFunc)
        {
            return pipe.Then(new SimpleActivity<TOut>(pipeCode,exeFunc));
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
        public static IPipelineConnector<TIn, TOut> ThenWithActivity<TIn, TOut, TNextResult>(this IPipelineConnector<TIn, TOut> pipe, string pipeCode,
            Func<TOut, Task<TrafficSignal<TNextResult>>> exeFunc)
        {
            return pipe.Then(new SimpleActivity<TOut, TNextResult>(pipeCode,exeFunc));
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
        public static IPipelineConnector<TIn, TResult> ThenWithEffectActivity<TIn, TOut, TResult>(
            this IPipelineConnector<TIn, TOut> pipe, string pipeCode,
            Func<Task<TrafficSignal<TResult>>> exeFunc)
        {
            return pipe.Then(new SimpleEffectActivity<TResult>(pipeCode,exeFunc));
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TPassivePara"></typeparam>
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
        public static IPipelineConnector<TIn, TResult> ThenWithEffectActivity<TIn, TPassivePara, TResult>(
            this IPipelineConnector<TIn, TPassivePara> pipe, string pipeCode,
            Func<TPassivePara, Task<TrafficSignal<TResult>>> exeFunc)
        {
            return pipe.Then(new SimpleEffectActivity<TPassivePara, TResult>(pipeCode,exeFunc));
        }

        //===  被动模式一般需要显示调用，简化隐藏

        /////  <summary>
        /////   追加活动管道
        /////  </summary>
        /////  <typeparam name="TPassivePara"></typeparam>
        /////  <typeparam name="TResult"></typeparam>
        /////  <typeparam name="TIn"></typeparam>
        /////  <typeparam name="TOut"></typeparam>
        /////  <param name="pipe"></param>
        /////  <param name="exeFunc">
        ///// 执行委托
        /////  参数：
        /////      当前活动上下文信息
        /////  结果：
        /////      TrafficSignal &lt;TResult &gt; -（活动是否处理成功，业务结果）
        /////          Green_Pass  - 流体自动流入后续管道
        /////          Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        /////          Red_Block - 触发Block，业务流不再向后续管道传递。
        /////  </param>
        /////  <param name="pipeCode"></param>
        /////  <returns></returns>
        //public static IPipelineAppender<TIn, TPassivePara> ThenWithPassiveActivity<TIn, TOut, TPassivePara, TResult>(
        //    this IPipelineAppender<TIn, TOut> pipe, string pipeCode,
        //    Func<TPassivePara, Task<TrafficSignal<TResult>>> exeFunc)
        //{
        //    return pipe.Then(new SimplePassiveActivity<TPassivePara, TResult>(pipeCode,exeFunc));
        //}


        /////  <summary>
        /////   追加活动管道
        /////  </summary>
        /////  <typeparam name="TPassivePara"></typeparam>
        /////  <typeparam name="TResult"></typeparam>
        /////  <typeparam name="TIn"></typeparam>
        /////  <typeparam name="TOut"></typeparam>
        /////  <param name="pipe"></param>
        /////  <param name="exeFunc">
        ///// 执行委托
        /////  参数：当前活动上下文信息
        /////  结果：
        /////      TrafficSignal &lt;TResult &gt; -（活动是否处理成功，业务结果）
        /////          Green_Pass  - 流体自动流入后续管道
        /////          Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        /////          Red_Block - 触发Block，业务流不再向后续管道传递。
        /////  </param>
        /////  <param name="pipeCode"></param>
        /////  <returns></returns>
        //public static IPipelineAppender<TIn, TResult> ThenWithPassiveEffectActivity<TIn, TOut, TPassivePara, TResult>(
        //    this IPipelineAppender<TIn, TOut> pipe, string pipeCode,
        //    Func<TPassivePara, Task<TrafficSignal<TResult>>> exeFunc)
        //{
        //    return pipe.Then(new SimplePassiveEffectActivity<TPassivePara, TResult>(pipeCode,exeFunc));
        //}
    }
}