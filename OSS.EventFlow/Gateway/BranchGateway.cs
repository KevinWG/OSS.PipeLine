using System.Threading.Tasks;
using OSS.EventFlow.Agent;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Gateway
{
    public class BranchGateway : BaseGateway
    {
        private readonly BaseAgent[] _nextAgents;

        public BranchGateway(BaseAgent[] nextAgents):base(GatewayType.Branch)
        {
            _nextAgents = nextAgents;
        }

        internal override async Task MoveSubNext( )
        {
            await MoveMulitAgents( _nextAgents);
        }
    }
}
