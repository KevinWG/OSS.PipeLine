using System.Threading.Tasks;
using OSS.EventFlow.Agent;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Gateway
{
    public class SecrialGateway:BaseGateway
    {
        private readonly BaseAgent _nextAgent;
        public SecrialGateway(BaseAgent nextAgent):base(GatewayType.Serial)
        {
            _nextAgent = nextAgent;
        }


        internal override  Task MoveSubNext( )
        {
            return MoveSingleAgents( _nextAgent);
        }
    }
}
