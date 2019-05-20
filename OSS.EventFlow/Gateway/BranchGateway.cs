using System.Threading.Tasks;
using OSS.EventFlow.Agent;
using OSS.EventFlow.Mos;
using OSS.EventNode.Interfaces;

namespace OSS.EventFlow.Gateway
{
    public class BranchGateway : BaseGateway
    {
        private readonly BaseAgent[] _nextAgents;

        public BranchGateway(BaseAgent[] nextAgents)
        {
            _nextAgents = nextAgents;
            GatewayType = GatewayType.Branch;
        }

  
        internal override async Task MoveNext(IExecuteData preData)
        {
            if (_nextAgents == null||(GatewayType == GatewayType.Branch && _nextAgents.Length == 1))
            {
                await MoveUnusualAgent(preData);
                return;
            }
            await MoveMulitAgents(preData, _nextAgents);
        }
    }
}
