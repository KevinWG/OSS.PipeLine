
#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventTask - 流体的多路聚合网关基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-27
*       
*****************************************************************************/

#endregion

using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Gateway
{
    /// <summary>
    /// 流体的多路聚合网关基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseAggregationGate<TContext> : BaseMatchGate<TContext>
        where TContext : FlowContext
    {
     
    }
}