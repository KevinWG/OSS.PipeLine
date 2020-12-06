#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  流体的分支网关基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-27
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Gateway
{
    /// <summary>
    /// 流体的分支网关基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseBranchGateway<TContext> : BasePipe<TContext>
        where TContext : FlowContext
    {
        protected BaseBranchGateway() : base(PipeType.Gateway)
        {
        }

        internal override async Task<bool> Through(TContext context)
        {
            var nextPipes = FilterAvailablePipes(_branchItems, context);
            if (nextPipes == null || !nextPipes.Any())
            {
                return false;
            }

            var parallelPipes = nextPipes.Select(p => p.InternalDeepThrough(context));
            await Task.WhenAll(parallelPipes);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="branchItems"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract IEnumerable<BasePipe<TContext>> FilterAvailablePipes(List<BasePipe<TContext>> branchItems,
            TContext context);

        private List<BasePipe<TContext>> _branchItems;

        public void AddBranch(BasePipe<TContext> branchItem)
        {
            if (_branchItems == null)
            {
                _branchItems = new List<BasePipe<TContext>>();
            }

            _branchItems.Add(branchItem);
        }

        public void AddBranches(IList<BasePipe<TContext>> branchItems)
        {
            if (_branchItems == null)
            {
                _branchItems = new List<BasePipe<TContext>>();
            }

            _branchItems.AddRange(branchItems);
        }
    }
}
