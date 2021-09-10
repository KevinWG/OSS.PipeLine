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
using System.Threading.Tasks;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static partial class PipeExtension
    {
        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
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
        public static SimpleActivity AppendActivity<TOut>(this IPipeAppender<TOut> pipe, Func<Task<TrafficSignal>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleActivity(pipeCode,exeFunc);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
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
        public static SimpleActivity<TOut> AppendActivity<TOut>(this IPipeAppender<TOut> pipe, 
            Func<TOut, Task<TrafficSignal>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleActivity<TOut>(pipeCode,exeFunc);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TResult"></typeparam>
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
        public static SimpleActivity<TOut,TResult> AppendActivity<TOut, TResult>(this IPipeAppender<TOut> pipe, 
            Func<TOut, Task<TrafficSignal<TResult>>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleActivity<TOut, TResult>(pipeCode,exeFunc);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }



        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TNextResult"></typeparam>
        /// <typeparam name="TOut"></typeparam>
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
        public static SimpleEffectActivity<TNextResult> AppendEffectActivity<TOut, TNextResult>(
            this IPipeAppender<TOut> pipe, 
            Func<Task<TrafficSignal<TNextResult>>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleEffectActivity<TNextResult>(pipeCode,exeFunc);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }



        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TPassivePara"></typeparam>
        /// <typeparam name="TResult"></typeparam>
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
        public static SimpleEffectActivity<TPassivePara, TResult> AppendEffectActivity<TPassivePara, TResult>(
            this IPipeAppender<TPassivePara> pipe, 
            Func<TPassivePara, Task<TrafficSignal<TResult>>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleEffectActivity<TPassivePara, TResult>(pipeCode,exeFunc);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        //===  被动模式一般需要显示调用，简化隐藏

        /////  <summary>
        /////   追加活动管道
        /////  </summary>
        /////  <typeparam name="TPassivePara"></typeparam>
        /////  <typeparam name="TResult"></typeparam>
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
        //public static SimplePassiveActivity<TPassivePara, TResult> AppendPassiveActivity<TOut,TPassivePara, TResult>(
        //    this IPipeAppender<TOut> pipe, 
        //    Func<TPassivePara, Task<TrafficSignal<TResult>>> exeFunc, string pipeCode = null)
        //{
        //    var nextPipe = new SimplePassiveActivity<TPassivePara, TResult>(pipeCode,exeFunc);
        //    pipe.InterAppend(nextPipe);
        //    return nextPipe;
        //}


        /////  <summary>
        /////   追加活动管道
        /////  </summary>
        /////  <typeparam name="TPassivePara"></typeparam>
        /////  <typeparam name="TResult"></typeparam>
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
        //public static SimplePassiveEffectActivity<TPassivePara, TResult> AppendPassiveEffectActivity<TOut,TPassivePara, TResult>(
        //    this IPipeAppender<TOut> pipe, 
        //    Func<TPassivePara, Task<TrafficSignal<TResult>>> exeFunc, string pipeCode = null)
        //{
        //    var nextPipe = new SimplePassiveEffectActivity<TPassivePara, TResult>(pipeCode,exeFunc);
        //    pipe.InterAppend(nextPipe);
        //    return nextPipe;
        //}


    }
}