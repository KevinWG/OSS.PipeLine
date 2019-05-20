using System.Threading.Tasks;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.EventFlow.Agent;
using OSS.EventFlow.Mos;
using OSS.EventNode.Interfaces;

namespace OSS.EventFlow.Gateway
{
    public abstract class BaseGateway
    {
        public GatewayType GatewayType { get; internal set; }
        public BaseAgent UnusualAgent { get; set; }


        /// <summary>
        ///  聚合控制检查是否满足条件
        /// </summary>
        /// <param name="preData"></param>
        /// <returns>true - 满足条件，false- 不能满足条件</returns>
        protected virtual Task<bool> AggregateCheck(IExecuteData preData)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        ///  聚合控制释放，不再检查
        /// </summary>
        /// <param name="preData"></param>
        /// <returns></returns>
        protected virtual Task<bool> AggregateRelease(IExecuteData preData)
        {
            return Task.FromResult(true);
        }


        internal abstract Task MoveNext(IExecuteData preData);
 

        internal async Task MoveMulitAgents(IExecuteData preData, BaseAgent[] agents)
        {
            if (agents == null || agents.Length < 1)
            {
                await MoveUnusualAgent(preData);
                return;
            }
          
            foreach (var ag in agents)
            {
                await ag.MoveIn(preData);
            }
        }

        internal async Task MoveSingleAgents(IExecuteData preData, BaseAgent agent)
        {
            if (agent == null)
            {
                await MoveUnusualAgent(preData);
                return;
            }

            await agent.MoveIn(preData);

        }

        internal async Task MoveUnusualAgent(IExecuteData preData)
        {
            if (UnusualAgent == null)
                throw new ResultException(SysResultTypes.ApplicationError, "UnusualAgent is null!");
            await UnusualAgent.MoveIn(preData);
        }

        //internal abstract BaseAgent[]  GetNextAgentMaps();
    }
}
