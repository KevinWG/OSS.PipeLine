using System;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline;

public static class BranchExtension
{
    /// <summary>
    ///   添加条件分支
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="TNextOut"></typeparam>
    /// <param name="pipe"></param>
    /// <param name="nextPipe"></param>
    /// <param name="branchCondition"></param>
    /// <returns></returns>
    public static IPipe<TOut, TNextOut> AppendCondition<TOut, TNextOut>(this BaseBranchGateway<TOut> pipe, Func<TOut, bool> branchCondition, IPipe<TOut, TNextOut> nextPipe)
    {
        pipe.SetCondition(nextPipe, branchCondition);
        pipe.InterAppend(nextPipe);
        return nextPipe;
    }

    /// <summary>
    ///  添加条件分支
    /// </summary>
    /// <typeparam name="TNextOut"></typeparam>
    /// <typeparam name="TOut">当前分支输出类型</typeparam>
    /// <param name="pipe"></param>
    /// <param name="nextPipe"></param>
    /// <param name="branchCondition"></param>
    /// <returns></returns>
    public static IPipe<Empty, TNextOut> Append<TOut,TNextOut>(this BaseBranchGateway<TOut> pipe, Func<TOut, bool> branchCondition, IPipe<Empty, TNextOut> nextPipe)
    {
        pipe.SetCondition(nextPipe, branchCondition);
        pipe.InterAppend(nextPipe);
        return nextPipe;
    }
}