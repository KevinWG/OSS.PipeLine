#region Copyright (C) 2021 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  默认活动提供者接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2021-01-24
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;

namespace OSS.PipeLine.Impls.Interface
{
    /// <summary>
    ///  默认活动提供者接口
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IActivityProvider<in TContext>
    //where TContext : IPipeContext
    {
        /// <summary>
        ///  执行
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<bool> Executing(TContext data);
    }
}