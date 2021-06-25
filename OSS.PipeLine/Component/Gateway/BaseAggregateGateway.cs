
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

using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline
{
    /// <summary>
    /// 流体的多路聚合网关基类
    /// the aggregative gate of flow
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseAggregateGateway<TContext> : BaseThreeWayPipe<TContext, TContext, TContext>
    {
        /// <summary>
        ///  流体的多路聚合网关基类构造函数
        /// </summary>
        protected BaseAggregateGateway() : base(PipeType.AggregateGateway)
        {
            _aggregatePipes = new List<IPipe>();
        }

        /// <summary>
        ///  是否触发通过
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prePipeCode">当前业务上游节点</param>
        /// <param name="allPrePipes">聚合到当前管道的所有上游节点</param>
        /// <returns></returns>
        protected abstract Task<TrafficSignal> Switch(TContext context, string prePipeCode, IList<IPipe> allPrePipes);

        internal override async Task<TrafficResult<TContext, TContext>> InterProcessPackage(TContext context,
            string prePipeCode)
        {
            var trafficSignal = await Switch(context, prePipeCode, _aggregatePipes);
            return new TrafficResult<TContext, TContext>(trafficSignal,
                trafficSignal.signal == SignalFlag.Red_Block ? PipeCode : string.Empty, context, context);
        }

        private readonly IList<IPipe> _aggregatePipes;

        private object _lockObj = new object();

        /// <inheritdoc />
        internal override void InterAppendTo(IPipe prePipe)
        {
            lock (_lockObj)
            {
                _aggregatePipes.Add(prePipe);
            }
        }
    }
}