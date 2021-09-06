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
    /// 管道对外执行接口（输入输出）
    /// </summary>
    public interface IPipeExecutor<in TPara, TResult> : IPipeMeta
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Task<TResult> Execute(TPara para);
    }

    /// <summary>
    /// 管道对外执行接口（无输入有输出）
    /// </summary>
    public interface IPipeOutputExecutor<TOut> : IPipeExecutor<Empty, TOut>
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <returns></returns>
        Task<TOut> Execute();
    }



    /// <summary>
    /// 管道对外执行接口（无输入无输出）
    /// </summary>
    public interface IPipeExecutor : IPipeExecutor<Empty, Empty>
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <returns></returns>
        Task Execute();
    }


    /// <summary>
    /// 管道对外执行接口（有输入无输出）
    /// </summary>
    public interface IPipeInputExecutor<in TIn>
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <returns></returns>
        Task Execute(TIn para);
    }
}
