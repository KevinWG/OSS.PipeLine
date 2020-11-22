#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  排他网关基类
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
    //public class ExclusiveGateway : BaseGateway
    //{
    //    //private readonly Func< Task<BaseMsgTunnel>> _exclusiveFunc;

    //    //public ExclusiveGateway(Func< Task<BaseMsgTunnel>> exclusiveFunc):base(GatewayType.ExclusiveSerial)
    //    //{
    //    //    _exclusiveFunc = exclusiveFunc;
    //    //}

    //    //protected virtual Task<BaseMsgTunnel> GetExclusiveAgent()
    //    //{
    //    //    return Task.FromResult<BaseMsgTunnel>(null);
    //    //}
        
     
    //    ////internal override BaseAgent[] GetNextAgentMaps()
    //    ////{
    //    ////    return new[] {_nextAgent};
    //    ////}
    //    //internal override async Task MoveSubNext()
    //    //{
    //    //    var agent=await (_exclusiveFunc?.Invoke() ?? GetExclusiveAgent());
    //    //    await MoveSingleAgents( agent);
    //    //}
    //}
}
