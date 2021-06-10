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
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc">
        /// 执行委托，返回的处理结果：
        /// False - 触发Block，业务流不再向后续管道传递。
        /// True  - 流体自动流入后续管道
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleActivity AppendActivity<OutContext>(this IOutPipeAppender<OutContext> pipe,
            Func<Task<TrafficSignal>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleActivity(exeFunc, pipeCode);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc">
        /// 执行委托
        /// 参数：当前活动上下文（会继续传递给下一个节点）
        /// 结果：
        ///     False - 触发Block，业务流不再向后续管道传递。
        ///     True  - 流体自动流入后续管道
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleActivity<OutContext> AppendActivity<OutContext>(this IOutPipeAppender<OutContext> pipe,
            Func<OutContext, Task<TrafficSignal>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleActivity<OutContext>(exeFunc, pipeCode);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc">
        /// 执行委托
        /// 结果：
        /// (bool traffic_signal,TResult result)-（活动是否处理成功，业务结果）
        ///    traffic_signal：
        ///        False - 触发Block，业务流不再向后续管道传递。
        ///        True  - 流体自动流入后续管道
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleEffectActivity<TResult> AppendEffectActivity<TResult>(
            this IOutPipeAppender<EmptyContext> pipe,
            Func<Task<(TrafficSignal traffic_signal, TResult result)>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleEffectActivity<TResult>(exeFunc, pipeCode);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TFuncPara"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc">
        /// 执行委托
        /// 参数：
        ///     当前活动上下文信息
        /// 结果：
        ///     (bool traffic_signal,TResult result)-（活动是否处理成功，业务结果）
        ///         traffic_signal：
        ///             False - 触发Block，业务流不再向后续管道传递。
        ///             True  - 流体自动流入后续管道
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleEffectActivity<TFuncPara, TResult> AppendEffectActivity<TFuncPara, TResult>(
            this IOutPipeAppender<TFuncPara> pipe,
            Func<TFuncPara, Task<(TrafficSignal traffic_signal, TResult result)>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleEffectActivity<TFuncPara, TResult>(exeFunc, pipeCode);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }



        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TFuncPara"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc">
        ///执行委托
        /// 参数：
        ///     当前活动上下文信息
        /// 结果：
        ///     (bool traffic_signal,TResult result)-（活动是否处理成功，业务结果）
        ///         traffic_signal：
        ///             False - 触发Block，业务流不再向后续管道传递。
        ///             True  - 流体自动流入后续管道
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleFuncActivity<TFuncPara, TResult> AppendFuncActivity<TFuncPara, TResult>(
            this IOutPipeAppender<TFuncPara> pipe,
            Func<TFuncPara, Task<(TrafficSignal traffic_signal, TResult result)>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleFuncActivity<TFuncPara, TResult>(exeFunc, pipeCode);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TFuncPara"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc">
        ///执行委托
        /// 参数：当前活动上下文信息
        /// 结果：
        /// (bool traffic_signal,TResult result)-（活动是否处理成功，业务结果）
        /// traffic_signal：
        ///     False - 触发Block，业务流不再向后续管道传递。
        ///     True  - 流体自动流入后续管道
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleFuncEffectActivity<TFuncPara, TResult> AppendFuncEffectActivity<TFuncPara, TResult>(
            this IOutPipeAppender<TFuncPara> pipe,
            Func<TFuncPara, Task<(TrafficSignal traffic_signal, TResult result)>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleFuncEffectActivity<TFuncPara, TResult>(exeFunc, pipeCode);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


    }
}