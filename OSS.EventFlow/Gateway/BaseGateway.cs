using System;
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


        internal virtual Task<BaseAgent> GetAgnet(IExecuteData preData)
        {
            return Task.FromResult<BaseAgent>(null);
        }

        internal virtual Task<BaseAgent[]> GetAgnets(IExecuteData preData)
        {
            return Task.FromResult<BaseAgent[]>(null);
        }

        internal async Task MoveNext(IExecuteData preData)
        {
            var check = await AggregateCheck(preData);
            if (!check)
            {
                var release = await AggregateRelease(preData);
                if (release)
                {
                    await MoveUnusual(preData);
                    return;
                }
            }
            if (GatewayType==GatewayType.ExclusiveSerial
                || GatewayType == GatewayType.Serial)
            {
                var agent =await GetAgnet(preData);
                if (agent==null)
                {
                    await MoveUnusual(preData);
                    return;
                }

                await agent.MoveIn(preData);
                return;
            }
            var agents = await GetAgnets(preData);
            if (agents == null||agents.Length<1)
            {
                await MoveUnusual(preData);
                return;
            }

            if (GatewayType==GatewayType.Branch&& agents.Length==1)
            {
                await MoveUnusual(preData);
                return;
            }
            foreach (var ag in agents)
            {
                await ag.MoveIn(preData);
            }
        }

        private async Task MoveUnusual(IExecuteData preData)
        {
            if (UnusualAgent == null)
                throw new ResultException(SysResultTypes.ApplicationError, "UnusualAgent is null!");
            await UnusualAgent.MoveIn(preData);
        }

        //internal abstract BaseAgent[]  GetNextAgentMaps();
    }
}
