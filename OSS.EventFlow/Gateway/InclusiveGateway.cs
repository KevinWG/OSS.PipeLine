using System;
using System.Threading.Tasks;
using OSS.EventFlow.Agent;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Gateway
{
    public class InclusiveGateway : BaseGateway
    {
        private Func< Task<BaseAgent[]>> _inclusiveFunc { get; }

     
        public InclusiveGateway(Func< Task<BaseAgent[]>> inclusiveFunc):base(GatewayType.InclusiveBranch)
        {
            _inclusiveFunc = inclusiveFunc;
        }

        
        protected virtual Task<BaseAgent[]> GetAgents( )
        {
            return Task.FromResult<BaseAgent[]>(null);
        }

   
        internal override async Task MoveSubNext( )
        {
            var agents = await (_inclusiveFunc?.Invoke() ?? GetAgents());
            await MoveMulitAgents(agents);
        }
    }
}
