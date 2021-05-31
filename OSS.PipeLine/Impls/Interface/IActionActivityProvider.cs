#region Copyright (C) 2021 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  默认外部Action活动基类 的提供者接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2021-01-24
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Impls.Interface
{
    /// <summary>
    /// 默认外部Action活动基类 的提供者接口
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IActionActivityProvider<in TContext, TResult>
        //where TContext : IPipeContext
    {
        /// <summary>
        /// 执行活动
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isBlocked"></param>
        /// <returns></returns>
        Task<TResult> Executing(TContext data, out bool isBlocked);

    }


    /// <inheritdoc />
    public interface IActionActivityWithNoticeProvider<in TContext, TResult> :IActionActivityProvider< TContext, TResult>
        //where TContext : IPipeContext
    {
        /// <summary>
        ///  消息进入通知
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<bool> Notice(TContext data) ;
    }
}