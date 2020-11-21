using System;
using OSS.EventFlow.Interfaces;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Gateway
{
    public class BaseGateway:IPipe
    {
        public void InterInput(FlowContext context)
        {
            throw new NotImplementedException();
        }

        public void InterOutput(FlowContext context)
        {
            throw new NotImplementedException();
        }
    }


    //    public abstract class BaseGateway
    //    {
    //        public GatewayType GatewayType { get; internal set; }

    //        protected BaseGateway(GatewayType gatewayType)
    //        {
    //            GatewayType = gatewayType;
    //        }

    //        /// <summary>
    //        ///  聚合控制检查是否满足条件
    //        /// </summary>
    //        /// <param name="preData"></param>
    //        /// <returns>true - 满足条件，false- 不能满足条件</returns>
    //        protected internal virtual Task<bool> AggregateCheck()
    //        {
    //            return Task.FromResult(true);
    //        }

    //        /// <summary>
    //        ///  聚合控制释放，不再检查
    //        /// </summary>
    //        /// <param name="preData"></param>
    //        /// <returns></returns>
    //        protected internal virtual Task<bool> AggregateRelease()
    //        {
    //            return Task.FromResult(true);
    //        }

    //        internal async Task MoveNext()
    //        {
    //            var aCheck = await AggregateCheck();
    //            if (!aCheck)
    //            {
    //                var release = await AggregateRelease();
    //                if (release)
    //                {
    //                    return ;
    //                }
    //            }
    //            await MoveSubNext();
    //        }

    //        internal abstract Task MoveSubNext( );


    //        /// <summary>
    //        ///   多agent前进 
    //        /// </summary>
    //        /// <param name="preData"></param>
    //        /// <returns></returns>
    //        internal static async Task MoveMulitAgents( BaseMsgTunnel[] agents)
    //        {
    //            foreach (var ag in agents)
    //            {
    //                await ag.MoveIn();
    //            }
    //        }

    //        /// <summary>
    //        /// 单一agent前进
    //        /// </summary>
    //        /// <param name="preData"></param>
    //        /// <returns></returns>
    //        internal static  Task MoveSingleAgents( BaseMsgTunnel agent)
    //        {
    //            return agent.MoveIn();
    //        }

    //    }
}
