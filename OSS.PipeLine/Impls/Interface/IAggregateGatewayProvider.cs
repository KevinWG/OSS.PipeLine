#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-27
*       
*****************************************************************************/

#endregion


using System.Threading.Tasks;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Impls.Interface
{
    /// <summary>
    ///  聚合网关的提供者接口
    ///  the interface of AggregateGatewayProvider
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IAggregateGatewayProvider<TContext> 
        //where TContext : IPipeContext
    {
        /// <summary>
        ///  是否触发通过
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isBlocked"></param>
        /// <returns></returns>
        Task<bool> IfMatchCondition(TContext context, out bool isBlocked);
    }
}
