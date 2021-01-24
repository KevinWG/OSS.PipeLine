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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.EventFlow.Gateway.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Gateway
{
    /// <summary>
    /// 流体的分支网关基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseBranchGateway<TContext> : BasePipe<TContext>
        where TContext : IFlowContext
    {
        /// <summary>
        ///  流体的分支网关基类
        /// </summary>
        protected BaseBranchGateway() : base(PipeType.Gateway)
        {
        }

        internal override async Task<bool> Through(TContext context)
        {
            var nextPipes = FilterNextPipes(_branchItems, context);
            if (nextPipes == null || !nextPipes.Any())
            {
                return false;
            }

            var parallelPipes = nextPipes.Select(p => p.Start(context));
            await Task.WhenAll(parallelPipes);
            return true;
        }

        /// <summary>
        ///   过滤可分发下路的分支
        ///   filer available pipes that can go to next during the runtime;
        /// </summary>
        /// <param name="branchItems"></param>
        /// <param name="context"></param>
        /// <returns>如果为空，则触发block</returns>
        protected abstract IEnumerable<BasePipe<TContext>> FilterNextPipes(List<BasePipe<TContext>> branchItems,
            TContext context);



        private List<BasePipe<TContext>> _branchItems;

        /// <summary>
        ///   添加分支
        /// </summary>
        /// <param name="branchItem"></param>
        public void AddBranch(BasePipe<TContext> branchItem)
        {
            _branchItems ??= new List<BasePipe<TContext>>();

            _branchItems.Add(branchItem);
        }

        /// <summary>
        ///  添加分支列表
        /// </summary>
        /// <param name="branchItems"></param>
        public void AddBranches(IList<BasePipe<TContext>> branchItems)
        {
            _branchItems ??= new List<BasePipe<TContext>>();

            _branchItems.AddRange(branchItems);
        }
    }

    /// <summary>
    /// 流体的分支网关基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DefaultBranchGateway<TContext> : BaseBranchGateway<TContext>
        where TContext : IFlowContext
    {

        private readonly IBranchGatewayProvider<TContext> _provider;

        /// <summary>
        /// 流体的分支网关基类
        /// </summary>
        /// <param name="provider"></param>
        public DefaultBranchGateway(IBranchGatewayProvider<TContext> provider) 
        {
            _provider = provider;
        }

        /// <summary>
        ///   过滤可分发下路的分支
        ///   filer available pipes that can go to next during the runtime;
        /// </summary>
        /// <param name="branchItems"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override IEnumerable<BasePipe<TContext>> FilterNextPipes(List<BasePipe<TContext>> branchItems, TContext context) => _provider.FilterNextPipes(branchItems, context);
    }

}
