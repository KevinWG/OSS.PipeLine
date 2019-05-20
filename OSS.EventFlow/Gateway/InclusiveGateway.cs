using System;
using System.Threading.Tasks;
using OSS.EventFlow.Agent;
using OSS.EventFlow.Mos;
using OSS.EventNode.Interfaces;

namespace OSS.EventFlow.Gateway
{
    public class InclusiveGateway : BaseGateway
    {
        private Func<IExecuteData, Task<BaseAgent[]>> _inclusiveFunc { get; set; }

        public InclusiveGateway(Func<IExecuteData, Task<BaseAgent[]>> inclusiveFunc)
        {
            _inclusiveFunc = inclusiveFunc;
            GatewayType = GatewayType.InclusiveBranch;
        }

        
        protected virtual Task<BaseAgent[]> GetAgents(IExecuteData data)
        {
            return Task.FromResult<BaseAgent[]>(null);
        }

        internal override Task<BaseAgent[]> GetAgnets(IExecuteData preData)
        {
            return _inclusiveFunc?.Invoke(preData) ?? GetAgnets(preData);
        }
    }
}
