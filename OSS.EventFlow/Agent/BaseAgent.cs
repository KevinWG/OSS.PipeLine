using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventFlow.Mos;
using OSS.EventNode;
using OSS.EventNode.Interfaces;
using OSS.EventNode.MetaMos;
using OSS.EventNode.Mos;

namespace OSS.EventFlow.Agent
{

    public abstract class BaseAgent
    {
        public RouterType RouterType { get; internal set; }

        protected virtual Task MoveIn(IExecuteData preData, NodeMeta preNode)
        {
            return Task.CompletedTask;
        }



        internal abstract Task MoveNext(IExecuteData preData);
    }

    public abstract class BaseAgent<TTData, TTRes> : BaseAgent
        where TTData :class ,IExecuteData
        where TTRes : ResultMo, new()
    {

        public BaseNode<TTData, TTRes> WorkNode { get; internal set; }

        protected BaseAgent(IEventNode<TTData, TTRes> node)
        {
            RouterType = RouterType.Serial;
        }


        public  Task<NodeResp<TTRes>> Process(TTData data)
        {
            return WorkNode.Process(data);
        }

        public Task<NodeResp<TTRes>> Process(TTData data, int triedTimes, params string[] taskIds)
        {
            return WorkNode.Process(data,triedTimes,taskIds);
        }
    }

}
