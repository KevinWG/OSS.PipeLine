

//using System.Threading.Tasks;
//using OSS.EventFlow.Gateway;

using System.Threading.Tasks;

namespace OSS.EventFlow.Agent
{
    public abstract class BaseAgent
    {
        public virtual Task MoveIn()
        {
            return Task.CompletedTask;
        }
    }

//    public abstract class BaseAgent<TTData, TTRes> : BaseAgent
//        where TTData :class ,
//        where TTRes : class, new()
//    {

//        private readonly BaseGateway _gateway;

//        protected BaseAgent(IEventNode<TTData, TTRes> node, BaseGateway gateway)
//        {
//            _workNode = node;
//            _gateway = gateway;
//        }


//        public  Task<GroupTaskResp<TTRes>> Process(TTData data)
//        {
//            return Process(data,0);
//        }

//        public async Task<GroupTaskResp<TTRes>> Process(TTData data, int triedTimes, params string[] taskIds)
//        {
//            var nodeRes= await _workNode.Process(data,triedTimes,taskIds);
//            await _gateway.MoveNext(data);
//            return nodeRes;
//        }
//    }

}
