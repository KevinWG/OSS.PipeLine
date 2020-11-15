using System;
using System.Threading.Tasks;
using OSS.EventFlow.Agent;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Gateway
{
    public class ExclusiveGateway : BaseGateway
    {
        private readonly Func< Task<BaseAgent>> _exclusiveFunc;

        public ExclusiveGateway(Func< Task<BaseAgent>> exclusiveFunc):base(GatewayType.ExclusiveSerial)
        {
            _exclusiveFunc = exclusiveFunc;
        }

        protected virtual Task<BaseAgent> GetExclusiveAgent()
        {
            return Task.FromResult<BaseAgent>(null);
        }
        
     
        //internal override BaseAgent[] GetNextAgentMaps()
        //{
        //    return new[] {_nextAgent};
        //}
        internal override async Task MoveSubNext()
        {
            var agent=await (_exclusiveFunc?.Invoke() ?? GetExclusiveAgent());
            await MoveSingleAgents( agent);
        }
    }
}
