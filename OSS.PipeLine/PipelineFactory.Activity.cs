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


using OSS.Pipeline.Interface;
using System;
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <summary>
    ///  pipeline 生成器
    /// </summary>
    public static partial class PipelineFactory
    {
        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="exeFunc">
        /// 执行委托，返回的处理结果：
        ///     TrafficSignal -（活动是否处理成功，业务结果）
        ///         Green_Pass  - 流体自动流入后续管道
        ///         Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///         Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static IPipelineAppender<Empty, Empty> StartWithActivity<OutContext>(
            Func<Task<TrafficSignal>> exeFunc, string pipeCode = null)
        {
            return  Start(new SimpleActivity(exeFunc, pipeCode));
        }
        
        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
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
        public static IPipelineAppender<OutContext, OutContext> StartWithActivity<OutContext>(
            Func<OutContext, Task<TrafficSignal>> exeFunc, string pipeCode = null)
        {
            return Start(new SimpleActivity<OutContext>(exeFunc, pipeCode));
        
        }

        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
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
        public static IPipelineAppender<Empty, TResult> StartWithEffectActivity<TResult>(
            Func<Task<TrafficSignal<TResult>>> exeFunc, string pipeCode = null)
        {
            return Start(new SimpleEffectActivity<TResult>(exeFunc, pipeCode));
     
        }



        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TFuncPara"></typeparam>
        /// <typeparam name="TResult"></typeparam>
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
        public static IPipelineAppender<TFuncPara, TResult> StartWithEffectActivity<TFuncPara, TResult>(
            Func<TFuncPara, Task<TrafficSignal<TResult>>> exeFunc, string pipeCode = null)
        {
            return Start(new SimpleEffectActivity<TFuncPara, TResult>(exeFunc, pipeCode));
        
        }



        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TFuncPara"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="exeFunc">
        ///执行委托
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
        public static IPipelineAppender<Empty, TFuncPara> StartWithFuncActivity<TFuncPara, TResult>(
            Func<TFuncPara, Task<TrafficSignal<TResult>>> exeFunc, string pipeCode = null)
        {
            return Start(new SimpleFuncActivity<TFuncPara, TResult>(exeFunc, pipeCode));
       
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TFuncPara"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="exeFunc">
        ///执行委托
        /// 参数：当前活动上下文信息
        /// 结果：
        ///     TrafficSignal &lt;TResult &gt; -（活动是否处理成功，业务结果）
        ///         Green_Pass  - 流体自动流入后续管道
        ///         Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///         Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static IPipelineAppender<Empty, TResult> StartWithFuncEffectActivity<TFuncPara, TResult>(
            Func<TFuncPara, Task<TrafficSignal<TResult>>> exeFunc, string pipeCode = null)
        {
            return Start(new SimpleFuncEffectActivity<TFuncPara, TResult>(exeFunc, pipeCode));
        }


    }
}
