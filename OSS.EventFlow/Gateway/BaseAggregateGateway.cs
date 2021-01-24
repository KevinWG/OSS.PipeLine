
#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 流体的多路聚合网关基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-27
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.EventFlow.Gateway.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Gateway
{
    /// <summary>
    /// 流体的多路聚合网关基类
    /// the aggregative gate of flow
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseAggregateGateway<TContext> : BaseSinglePipe<TContext, TContext>
        where TContext : IFlowContext
    {
        /// <summary>
        ///  流体的多路聚合网关基类构造函数
        /// </summary>
        protected BaseAggregateGateway() : base(PipeType.Gateway)
        {
        }

        /// <summary>
        ///  是否触发通过
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isBlocked"></param>
        /// <returns></returns>
        protected abstract Task<bool> IfMatchCondition(TContext context, out bool isBlocked);


        internal override async Task<bool> Through(TContext context)
        {
            var throughRes = await IfMatchCondition(context, out var isBlocked);
            if (isBlocked)
            {
                return false;
            }

            if (throughRes)
            {
                await ToNextThrough(context);
            }
            return true;
        }
    }


    /// <summary>
    ///  聚合网关的默认实现类
    ///  the default implement of BaseAggregateGate
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DefaultAggregateGateway<TContext> : BaseAggregateGateway<TContext>
        where TContext : IFlowContext
    {
        private readonly IAggregateGatewayProvider<TContext> _provider;
        /// <summary>
        /// 聚合网关的默认实现 
        /// the default implement of BaseAggregateGate
        /// </summary>
        /// <param name="provider"></param>
        public DefaultAggregateGateway(IAggregateGatewayProvider<TContext> provider)
        {
            _provider = provider;
        }

        /// <summary>
        ///  是否匹配通过条件
        ///  if match the condition to next pipe
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isBlocked"></param>
        /// <returns></returns>
        protected override Task<bool> IfMatchCondition(TContext context, out bool isBlocked)
        {
            return _provider.IfMatchCondition(context,out isBlocked);
        }
    }
}