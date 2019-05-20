using System.Threading.Tasks;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
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

        internal override Task<BaseAgent[]> GetAgnets(IExecuteData preData)
        {
            return Task.FromResult(_nextAgents);
        }
    }
}
