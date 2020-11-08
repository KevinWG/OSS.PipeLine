using System.Threading.Tasks;
using OSS.EventFlow.Agent;
using OSS.EventFlow.Mos;
using OSS.EventNode.Interfaces;

namespace OSS.EventFlow.Gateway
{
    public class SecrialGateway:BaseGateway
    {
        private readonly BaseAgent _nextAgent;
        public SecrialGateway(BaseAgent nextAgent):base(GatewayType.Serial)
        {
            _nextAgent = nextAgent;
        }


        internal override  Task MoveSubNext(IExecuteData preData)
        {
            return MoveSingleAgents(preData, _nextAgent);
        }
    }
}
