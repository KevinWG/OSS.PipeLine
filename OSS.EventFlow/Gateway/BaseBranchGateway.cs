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

        internal override async Task Through(TContext context)
        {
            var parallelPipes = GetAvailablePipes(context).Select(p => p.Through(context));
            await Task.WhenAll(parallelPipes);
        }

        private IEnumerable<BasePipe<TContext>> GetAvailablePipes(TContext context)
        {
            return _branchItems.Where(b => b.pipeSwitcher.Invoke(context)).Select(b => b.pipe);
        }

        private IList<(BasePipe<TContext> pipe, Func<TContext, bool> pipeSwitcher)> _branchItems;

        public void Branch((BasePipe<TContext> pipe, Func<TContext, bool> pipeSwitcher) branchItem)
        {
            if (_branchItems == null)
            {
                _branchItems = new List<(BasePipe<TContext>, Func<TContext, bool>)>();
            }

            _branchItems.Add(branchItem);
        }
    }
}
