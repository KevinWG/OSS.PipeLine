#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  管道执行器接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;

namespace OSS.Pipeline.Interface
{
   
    /// <summary>
    /// 管道对外执行接口
    /// </summary>
    public interface IPipeExecutor<in TInContext, THandleResult, TOut> : IPipe
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Task<THandleResult> Execute(TInContext para);
    }


    /// <summary>
    /// 管道对外执行接口
    /// </summary>
    public interface IPipeExecutor<in TInContext, TOut> : IPipe, IPipeExecutor<TInContext, TOut, TOut>
    {
    }

    /// <summary>
    /// 管道对外执行接口
    /// </summary>
    public interface IPipeExecutor<TOut> : IPipe, IPipeExecutor<Empty, TOut>
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <returns></returns>
        Task<TOut> Execute();
    }

    /// <summary>
    /// 管道对外执行接口
    /// </summary>
    public interface IPipeFuncExecutor<in TFuncPara, TFuncResult> : IPipe
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Task<TFuncResult> Execute(TFuncPara para);
    }
}
