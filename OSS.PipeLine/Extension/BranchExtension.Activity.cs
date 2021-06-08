using System;
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static partial class BranchExtension
    {
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
        public static BaseActivity<OutContext> AppendActivity<OutContext>(this BaseBranchGateway<OutContext> pipe,
            Func<OutContext, Task<bool>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new InterActivity<OutContext>(exeFunc, pipeCode);
            pipe.AddBranch(nextPipe);
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
        ///     (bool is_ok,TResult result)-（活动是否处理成功，业务结果）
        ///         is_ok：
        ///             False - 触发Block，业务流不再向后续管道传递。
        ///             True  - 流体自动流入后续管道
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseEffectActivity<TFuncPara, TResult> AppendEffectActivity<TFuncPara, TResult>(
            this BaseBranchGateway<TFuncPara> pipe,
            Func<TFuncPara, Task<(bool is_ok, TResult result)>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new InterEffectActivity<TFuncPara, TResult>(exeFunc, pipeCode);
            pipe.AddBranch(nextPipe);
            return nextPipe;
        }


    }
}