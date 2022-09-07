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
        /// <typeparam name="TOut"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="branchCondition">分支条件判断</param>
        /// <param name="exeFunc"> 执行委托 </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleActivity AppendActivity<TOut>(
            this IBranchGateway<TOut> pipe, Func<TOut, bool> branchCondition,
            Func<Task<TrafficSignal>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleActivity(exeFunc, pipeCode);

            pipe.SetCondition(nextPipe, branchCondition);
            pipe.InterAppend(nextPipe);

            return nextPipe;
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="branchCondition">分支条件判断</param>
        /// <param name="exeFunc"> 执行委托 </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleActivity<TOut> AppendActivity<TOut>(
            this IBranchGateway<TOut> pipe, Func<TOut, bool> branchCondition,
            Func<TOut, Task<TrafficSignal>> exeFunc,
            string pipeCode = null)
        {
            var nextPipe = new SimpleActivity<TOut>(exeFunc, pipeCode);

            pipe.SetCondition(nextPipe, branchCondition);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TNextRes"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="branchCondition">分支条件判断</param>
        /// <param name="exeFunc"> 执行委托 </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleActivity<TOut, TNextRes> AppendActivity<TOut, TNextRes>(
            this IBranchGateway<TOut> pipe, Func<TOut, bool> branchCondition,
            Func<TOut, Task<TrafficSignal<TNextRes>>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleActivity<TOut, TNextRes>(exeFunc, pipeCode);

            pipe.SetCondition(nextPipe, branchCondition);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TNextRes"></typeparam>
        /// <typeparam name="TNextOut"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="branchCondition">分支条件判断</param>
        /// <param name="exeFunc"> 执行委托 </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleActivity<TOut, TNextRes, TNextOut> AppendActivity<TOut, TNextRes, TNextOut>(
            this IBranchGateway<TOut> pipe, Func<TOut, bool> branchCondition,
            Func<TOut, Task<TrafficSignal<TNextRes, TNextOut>>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleActivity<TOut, TNextRes, TNextOut>(exeFunc, pipeCode);

            pipe.SetCondition(nextPipe, branchCondition);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TNextRes"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="branchCondition">分支条件判断</param>
        /// <param name="exeFunc"> 执行委托 </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleEffectActivity<TNextRes> AppendEffectActivity<TOut, TNextRes>(
            this IBranchGateway<TOut> pipe, Func<TOut, bool> branchCondition,
            Func<Task<TrafficSignal<TNextRes>>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleEffectActivity<TNextRes>(exeFunc, pipeCode);

            pipe.SetCondition(nextPipe, branchCondition);
            pipe.InterAppend(nextPipe);

            return nextPipe;
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TNextRes"></typeparam>
        /// <param name="pipe"></param>
        ///<param name="branchCondition">分支条件判断</param>
        /// <param name="exeFunc"> 执行委托 </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleEffectActivity<TOut, TNextRes> AppendEffectActivity<TOut, TNextRes>(
            this IBranchGateway<TOut> pipe, Func<TOut, bool> branchCondition,
            Func<TOut, Task<TrafficSignal<TNextRes>>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new SimpleEffectActivity<TOut, TNextRes>(exeFunc, pipeCode);

            pipe.SetCondition(nextPipe, branchCondition);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }
    }
}