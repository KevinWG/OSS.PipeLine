using System;
using System.Threading.Tasks;
using OSS.EventFlow.Agent;
using OSS.EventFlow.Mos;
using OSS.EventNode.Interfaces;

namespace OSS.EventFlow.Gateway
{
    public class ExclusiveGateway : BaseGateway
    {
        private readonly Func<IExecuteData, Task<BaseAgent>> _exclusiveFunc;

        public ExclusiveGateway():this(null)
        {
        }
        

        public ExclusiveGateway(Func<IExecuteData, Task<BaseAgent>> exclusiveFunc,BaseAgent[] nextMaps=null)
        {
            _exclusiveFunc = exclusiveFunc;
            GatewayType = GatewayType.ExclusiveSerial;
        }

        protected virtual Task<BaseAgent> GetExclusiveAgent(IExecuteData data)
        {
            return Task.FromResult<BaseAgent>(null);
        }
        
     
        //internal override BaseAgent[] GetNextAgentMaps()
        //{
        //    return new[] {_nextAgent};
        //}
        internal override async Task MoveSubNext(IExecuteData preData)
        {
            var agent=await (_exclusiveFunc?.Invoke(preData) ?? GetExclusiveAgent(preData));
            await MoveSingleAgents(preData, agent);
        }
    }
}
