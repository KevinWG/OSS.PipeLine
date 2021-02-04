using System.Collections.Generic;
using OSS.EventFlow.Gateway;
using OSS.EventFlow.Impls.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Impls
{
    /// <summary>
    /// 流体的分支网关基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DefaultBranchGateway<TContext> : BaseBranchGateway<TContext>
        where TContext : IPipeContext
    {

        private readonly IBranchGatewayProvider<TContext> _provider;

        /// <summary>
        /// 流体的分支网关基类
        /// </summary>
        /// <param name="provider"></param>
        public DefaultBranchGateway(IBranchGatewayProvider<TContext> provider) 
        {
            _provider = provider;
            AddBranches(_provider.GetAllBranches());
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