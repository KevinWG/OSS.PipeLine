#region Copyright (C) 2021 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 流体阻塞接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2021-01-24
*       
*****************************************************************************/

#endregion


using System.Threading.Tasks;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Interface
{
    /// <summary>
    ///  流体阻塞接口
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IPipeBlock<in TContext>
        where TContext : IFlowContext
    {
        /// <summary>
        /// 流体阻塞调用方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task Block(TContext context);
    }
}
