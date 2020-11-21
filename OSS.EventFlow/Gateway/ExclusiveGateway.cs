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
