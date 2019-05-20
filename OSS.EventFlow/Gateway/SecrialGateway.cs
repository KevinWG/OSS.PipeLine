using System.Threading.Tasks;
using OSS.EventFlow.Agent;
using OSS.EventFlow.Mos;
using OSS.EventNode.Interfaces;

namespace OSS.EventFlow.Gateway
{
    public class SecrialGateway:BaseGateway
    {
        private readonly BaseAgent _nextAgent;
        public SecrialGateway(BaseAgent nextAgent)
        {
            _nextAgent = nextAgent;
            GatewayType = GatewayType.Serial;
        }


        internal override Task<BaseAgent> GetAgnet(IExecuteData preData)
        {
            return Task.FromResult(_nextAgent);
        }
    }
}
