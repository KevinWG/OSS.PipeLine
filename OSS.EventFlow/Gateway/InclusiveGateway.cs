using System;
using System.Threading.Tasks;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Gateway
{
    public class InclusiveGateway : BaseGateway
    {
        //private Func< Task<BaseMsgTunnel[]>> _inclusiveFunc { get; }

     
        //public InclusiveGateway(Func< Task<BaseMsgTunnel[]>> inclusiveFunc):base(GatewayType.InclusiveBranch)
        //{
        //    _inclusiveFunc = inclusiveFunc;
        //}

        
        //protected virtual Task<BaseMsgTunnel[]> GetAgents( )
        //{
        //    return Task.FromResult<BaseMsgTunnel[]>(null);
        //}

   
        //internal override async Task MoveSubNext( )
        //{
        //    var agents = await (_inclusiveFunc?.Invoke() ?? GetAgents());
        //    await MoveMulitAgents(agents);
        //}
    }
}
