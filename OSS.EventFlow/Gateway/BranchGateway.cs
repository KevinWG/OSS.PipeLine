using System.Threading.Tasks;
using OSS.EventFlow.Agent;
using OSS.EventFlow.Mos;
using OSS.EventNode.Interfaces;

namespace OSS.EventFlow.Gateway
{
    public class BranchGateway : BaseGateway
    {
        private readonly BaseAgent[] _nextAgents;

        public BranchGateway(BaseAgent[] nextAgents):base(GatewayType.Branch)
        {
            _nextAgents = nextAgents;
        }

        internal override async Task MoveSubNext(IExecuteData preData)
        {
            await MoveMulitAgents(preData, _nextAgents);
        }
    }
}
