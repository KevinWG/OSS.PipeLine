using System.Threading.Tasks;
using OSS.EventFlow.Gateway;
using OSS.EventFlow.Impls.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Impls
{
    /// <summary>
    ///  聚合网关的默认实现类
    ///  the default implement of BaseAggregateGate
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DefaultAggregateGateway<TContext> : BaseAggregateGateway<TContext>
        //where TContext : IPipeContext
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