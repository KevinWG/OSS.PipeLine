#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  包含网关基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System;
using System.Threading.Tasks;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Gateway
{
    //public class InclusiveGateway : BaseGateway
    //{
    //    //private Func< Task<BaseMsgTunnel[]>> _inclusiveFunc { get; }

     
    //    //public InclusiveGateway(Func< Task<BaseMsgTunnel[]>> inclusiveFunc):base(GatewayType.InclusiveBranch)
    //    //{
    //    //    _inclusiveFunc = inclusiveFunc;
    //    //}

        
    //    //protected virtual Task<BaseMsgTunnel[]> GetAgents( )
    //    //{
    //    //    return Task.FromResult<BaseMsgTunnel[]>(null);
    //    //}

   
    //    //internal override async Task MoveSubNext( )
    //    //{
    //    //    var agents = await (_inclusiveFunc?.Invoke() ?? GetAgents());
    //    //    await MoveMulitAgents(agents);
    //    //}
    //}
}
