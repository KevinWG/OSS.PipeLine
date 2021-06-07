
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
using OSS.Pipeline.Base;

namespace OSS.Pipeline
{
    /// <summary>
    /// 流体的多路聚合网关基类
    /// the aggregative gate of flow
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseAggregateGateway<TContext>  : BaseStraightPipe<TContext, TContext>
    {
        /// <summary>
        ///  流体的多路聚合网关基类构造函数
        /// </summary>
        protected BaseAggregateGateway() : base(PipeType.AggregateGateway)
        {
        }

        /// <summary>
        ///  是否触发通过
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract Task<bool> IfMatchCondition(TContext context);


        internal override Task<bool> InterHandling(TContext context)
        {
            return IfMatchCondition(context);
        }
    }
}