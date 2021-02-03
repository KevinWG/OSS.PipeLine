#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  缓冲连接器的提供者
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using OSS.EventFlow.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Connector.Interface
{

    /// <summary>
    /// 默认缓冲连接器提供者
    /// </summary>
    public interface IBufferConnectorProvider<TContext> : IBufferConnectorProvider<TContext, TContext>
        where TContext : IFlowContext
    {
    }


    /// <summary>
    /// 默认缓冲连接器提供者
    /// </summary>
    public interface IBufferConnectorProvider<in InContext, out OutContext> : IBufferPush<InContext>, IConnectorProvider<InContext, OutContext>
        where InContext : IFlowContext
        where OutContext : IFlowContext
    {
    }


}
