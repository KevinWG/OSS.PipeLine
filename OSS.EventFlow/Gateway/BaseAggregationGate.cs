
#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventTask - 流体的多路聚合网关基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-27
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Gateway
{
    /// <summary>
    /// 流体的多路聚合网关基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseAggregationGate<TContext> : BaseSinglePipe<TContext, TContext>
        where TContext : FlowContext
    {
        protected BaseAggregationGate() : base(PipeType.Gateway)
        {
        }

        /// <summary>
        ///  是否触发通过
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isBlocked"></param>
        /// <returns></returns>
        protected abstract Task<bool> MatchThroughCondition(TContext context, out bool isBlocked);

        internal override async Task<bool> Through(TContext context)
        {
            var throughRes = await MatchThroughCondition(context, out var isBlocked);
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
}