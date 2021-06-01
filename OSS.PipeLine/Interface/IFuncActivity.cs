#region Copyright (C) 2021 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 被动委托活动接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2021-6-1
*       
*****************************************************************************/

#endregion


using OSS.PipeLine.Interface;
using System.Threading.Tasks;

namespace OSS.Pipeline.Interface
{
    /// <summary>
    /// 被动委托活动接口
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IFuncActivity<TContext, TResult> : IPipe
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<TResult> Execute(TContext data);
    }
}
