

using System.Threading.Tasks;
using OSS.EventFlow.Gateway;
using OSS.EventNode.Interfaces;
using OSS.EventNode.Mos;

namespace OSS.EventFlow.Agent
{
    public abstract class BaseAgent
    {
        public virtual Task MoveIn(IExecuteData preData)
        {
            return Task.CompletedTask;
        }
    }

    public abstract class BaseAgent<TTData, TTRes> : BaseAgent
        where TTData :class ,IExecuteData
        where TTRes : class, new()
    {
        private readonly IEventNode<TTData, TTRes> _workNode;
        private readonly BaseGateway _gateway;

        protected BaseAgent(IEventNode<TTData, TTRes> node, BaseGateway gateway)
        {
            _workNode = node;
            _gateway = gateway;
        }


        public  Task<NodeResp<TTRes>> Process(TTData data)
        {
            return Process(data,0);
        }

        public async Task<NodeResp<TTRes>> Process(TTData data, int triedTimes, params string[] taskIds)
        {
            var nodeRes= await _workNode.Process(data,triedTimes,taskIds);
            await _gateway.MoveNext(data);
            return nodeRes;
        }
    }

}
