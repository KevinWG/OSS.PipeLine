using System;
using System.Threading.Tasks;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.EventFlow.Mos;
using OSS.EventNode.Interfaces;

namespace OSS.EventFlow.Agent
{
    public static class EventAgentExtention
    {
        /// <summary>
        ///  设置顺序节点
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="nextAgents"></param>
        public static void WithGateway(this BaseAgent agent, )
        {
            if (nextAgents == null || nextAgents.Length == 0)
            {
                throw new ResultException(SysResultTypes.AppConfigError, ResultTypes.ParaError, "Next agent can't be null!");
            }

            agent.RouterType = nextAgents.Length == 1 ? GatewayType.Serial : GatewayType.Branch;

            agent.NextController = d => Task.FromResult(nextAgents);
            agent.NextAgentMaps = nextAgents;
        }
        


    }

}
