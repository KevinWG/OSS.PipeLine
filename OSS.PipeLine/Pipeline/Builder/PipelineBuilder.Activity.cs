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


using OSS.Pipeline.Pipeline.InterImpls.Connector;
using System;
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <summary>
    ///  pipeline 生成器
    /// </summary>
    public static partial class PipelineBuilder
    {
        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="exePassive">
        /// 执行委托，返回的处理结果：
        ///     TrafficSignal -（活动是否处理成功，业务结果）
        ///         Green_Pass  - 流体自动流入后续管道
        ///         Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///         Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static IPipelineConnector<Empty, Empty> StartWithActivity<OutContext>(string pipeCode,
            Func<Task<TrafficSignal>> exePassive)
        {
            return Start(new SimpleActivity(pipeCode, exePassive));
        }

        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="exePassive">
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
        public static IPipelineConnector<OutContext, OutContext> StartWithActivity<OutContext>(string pipeCode,
            Func<OutContext, Task<TrafficSignal>> exePassive)
        {
            return Start(new SimpleActivity<OutContext>(pipeCode, exePassive));
        }

        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <typeparam name="TNextResult"></typeparam>
        /// <param name="exePassive">
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
        public static IPipelineConnector<OutContext, OutContext> StartWithActivity<OutContext, TNextResult>(string pipeCode,
            Func<OutContext, Task<TrafficSignal<TNextResult>>> exePassive)
        {
            return Start(new SimpleActivity<OutContext, TNextResult>(pipeCode, exePassive));
        }



        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="exePassive">
        /// 执行委托
        /// 结果：
        ///     TrafficSignal &lt;TResult &gt; -（活动是否处理成功，业务结果）
        ///         Green_Pass  - 流体自动流入后续管道
        ///         Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///         Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static IPipelineConnector<Empty, TResult> StartWithEffectActivity<TResult>(string pipeCode,
            Func<Task<TrafficSignal<TResult>>> exePassive)
        {
            return Start(new SimpleEffectActivity<TResult>(pipeCode, exePassive));
        }



        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TPassivePara"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="exePassive">
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
        public static IPipelineConnector<TPassivePara, TResult> StartWithEffectActivity<TPassivePara, TResult>(string pipeCode,
            Func<TPassivePara, Task<TrafficSignal<TResult>>> exePassive)
        {
            return Start(new SimpleEffectActivity<TPassivePara, TResult>(pipeCode, exePassive));

        }

        //===  被动模式一般需要显示调用，简化隐藏

        ///// <summary>
        /////  追加活动管道
        ///// </summary>
        ///// <typeparam name="TPassivePara"></typeparam>
        ///// <typeparam name="TResult"></typeparam>
        ///// <param name="exePassive">
        /////执行委托
        ///// 参数：
        /////     当前活动上下文信息
        ///// 结果：
        /////     TrafficSignal &lt;TResult &gt; -（活动是否处理成功，业务结果）
        /////         Green_Pass  - 流体自动流入后续管道
        /////         Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        /////         Red_Block - 触发Block，业务流不再向后续管道传递。
        ///// </param>
        ///// <param name="pipeCode"></param>
        ///// <returns></returns>
        //public static IPipelineAppender<Empty, TPassivePara> StartWithPassiveActivity<TPassivePara, TResult>(string pipeCode,
        //    Func<TPassivePara, Task<TrafficSignal<TResult>>> exePassive)
        //{
        //    return Start(new SimplePassiveActivity<TPassivePara, TResult>(pipeCode,exePassive));

        //}


        ///// <summary>
        /////  追加活动管道
        ///// </summary>
        ///// <typeparam name="TPassivePara"></typeparam>
        ///// <typeparam name="TResult"></typeparam>
        ///// <param name="exePassive">
        /////执行委托
        ///// 参数：当前活动上下文信息
        ///// 结果：
        /////     TrafficSignal &lt;TResult &gt; -（活动是否处理成功，业务结果）
        /////         Green_Pass  - 流体自动流入后续管道
        /////         Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        /////         Red_Block - 触发Block，业务流不再向后续管道传递。
        ///// </param>
        ///// <param name="pipeCode"></param>
        ///// <returns></returns>
        //public static IPipelineAppender<Empty, TResult> StartWithPassiveEffectActivity<TPassivePara, TResult>(string pipeCode,
        //    Func<TPassivePara, Task<TrafficSignal<TResult>>> exePassive)
        //{
        //    return Start(new SimplePassiveEffectActivity<TPassivePara, TResult>(pipeCode,exePassive));
        //}


    }
}
