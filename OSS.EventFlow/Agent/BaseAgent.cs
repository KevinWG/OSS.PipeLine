using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventFlow.Gateway;
using OSS.EventNode;
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

        public virtual Task MoveOut(IExecuteData preData)
        {
            return Task.CompletedTask;
        }

        public BaseGateway Gateway { get; set; }
    }

    public abstract class BaseAgent<TTData, TTRes> : BaseAgent
        where TTData :class ,IExecuteData
        where TTRes : ResultMo, new()
    {
        public IEventNode<TTData, TTRes> WorkNode { get; internal set; }

        protected BaseAgent(IEventNode<TTData, TTRes> node)
        {
            WorkNode = node;
        }


        public  Task<NodeResp<TTRes>> Process(TTData data)
        {
            return Process(data,0);
        }

        public async Task<NodeResp<TTRes>> Process(TTData data, int triedTimes, params string[] taskIds)
        {
            var nodeRes= await WorkNode.Process(data,triedTimes,taskIds);

            var aCheck = await Gateway.AggregateCheck(preData);
            if (!aCheck)
            {
                var release = await Gateway.AggregateRelease(preData);
                if (release)
                {
                    await Gateway.MoveUnusualAgent(preData);
                    return;
                }
            }
            await Gateway.MoveSubNext(data);

            return nodeRes;
        }
    }

}
