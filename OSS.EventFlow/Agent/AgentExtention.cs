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
        ///// <summary>
        /////  设置顺序节点
        ///// </summary>
        ///// <param name="agent"></param>
        ///// <param name="nextAgents"></param>
        //public static void WithGateway(this BaseAgent agent,params BaseAgent[] nextAgents)
        //{
        //    if (nextAgents==null||nextAgents.Length==0)
        //    {
        //        throw new ResultException(SysResultTypes.AppConfigError,ResultTypes.ParaError,"Next agent can't be null!");
        //    }

        //    agent.RouterType = nextAgents.Length == 1 ? GatewayType.Serial : GatewayType.Branch;

        //    agent.NextController = d => Task.FromResult(nextAgents);
        //    agent.NextAgentMaps = nextAgents;
        //}

        //public static void WithGateway(this BaseAgent agent, 
        //    Func<IExecuteData, Task<BaseAgent>> serialController,
        //    BaseAgent[] nextMaps = null)
        //{
        //}

        //public static void WithGateway(this BaseAgent agent,
        //    Func<IExecuteData, Task<BaseAgent[]>> branchController,
        //    BaseAgent[] nextMaps = null)
        //{
        //    agent.RouterType = GatewayType.Branch;
        //}


        //public static void WithGateway(this BaseAgent agent, 
        //    Func<IExecuteData, Task<bool>> cycleController,
        //    params BaseAgent[] nextAgents)
        //{
        //    //agent.RouterType = RouterType.Cycle;
        //}

        //public static void WithGateway(this BaseAgent agent, 
        //    Func<IExecuteData, Task<bool>> cycleController,
        //    Func<IExecuteData, Task<BaseAgent>> next,
        //    BaseAgent[] nextMaps = null)
        //{
        //    //agent.RouterType = RouterType.Cycle;
        //}

        //public static void WithGateway(this BaseAgent agent, 
        //    Func<IExecuteData, Task<bool>> condition,
        //    Func<IExecuteData, Task<BaseAgent[]>> branchController,
        //    BaseAgent[] nextMaps = null)
        //{
        //    //agent.RouterType = RouterType.Cycle;
        //}


    }

}
