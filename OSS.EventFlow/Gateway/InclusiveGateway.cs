using System;
using System.Threading.Tasks;
using OSS.EventFlow.Agent;
using OSS.EventFlow.Mos;
using OSS.EventNode.Interfaces;

namespace OSS.EventFlow.Gateway
{
    public class InclusiveGateway : BaseGateway
    {
        private Func<IExecuteData, Task<BaseAgent[]>> _inclusiveFunc { get; }

     
        public InclusiveGateway(Func<IExecuteData, Task<BaseAgent[]>> inclusiveFunc):base(GatewayType.InclusiveBranch)
        {
            _inclusiveFunc = inclusiveFunc;
        }

        
        protected virtual Task<BaseAgent[]> GetAgents(IExecuteData data)
        {
            return Task.FromResult<BaseAgent[]>(null);
        }

   
        internal override async Task MoveSubNext(IExecuteData preData)
        {
            var agents = await (_inclusiveFunc?.Invoke(preData) ?? GetAgents(preData));
            await MoveMulitAgents(preData,agents);
        }
    }
}
