
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
        protected abstract Task<MatchCondition> IfMatchCondition(TContext context);


        internal override async Task<bool> InterHandling(TContext context)
        {
            var res = await IfMatchCondition(context);
            if (res== MatchCondition.MatchAndContinue)
            {
                await ToNextThrough(context);
            }
            return res == MatchCondition.NotMatchAndWait;//返回false触发block
        }
    }

    /// <summary>
    ///  匹配结果
    /// </summary>
    public enum MatchCondition
    {
        /// <summary>
        ///  匹配且继续
        /// </summary>
        MatchAndContinue=0,

        /// <summary>
        ///  不匹配也不继续向后流动
        /// </summary>
        NotMatchAndWait=10,

        /// <summary>
        ///  不匹配且触发阻塞
        /// </summary>
        NotMatchAndBlock=20
    }

}