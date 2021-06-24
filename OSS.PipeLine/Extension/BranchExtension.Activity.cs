#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  外部动作活动
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

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
        public static SimpleActivity<OutContext> AddActivityBranch<OutContext>(this BaseBranchGateway<OutContext> pipe,
            Func<OutContext, Task<TrafficSignal>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleActivity<OutContext>(exeFunc, pipeCode);
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
        ///   signal：
        ///     Green_Pass  - 流体自动流入后续管道
        ///     Yellow_Wait - 暂停执行，既不向后流动，也不触发Block。
        ///     Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleEffectActivity<TFuncPara, TResult> AddEffectActivityBranch<TFuncPara, TResult>(
            this BaseBranchGateway<TFuncPara> pipe,
            Func<TFuncPara, Task<TrafficSignal<TResult>>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleEffectActivity<TFuncPara, TResult>(exeFunc, pipeCode);
            pipe.AddBranch(nextPipe);
            return nextPipe;
        }


    }
}