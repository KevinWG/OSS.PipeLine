using OSS.EventFlow.Gateway;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Interface
{
    /// <summary>
    ///  管道链接器
    /// </summary>
    /// <typeparam name="TNextInContext"></typeparam>
    public interface IPipeAppender<TNextInContext>
        where TNextInContext : IPipeContext
    {
        /// <summary>
        /// 追加管道
        /// </summary>
        /// <param name="nextPipe"></param>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <returns>下个管道的追加器</returns>
        BaseSinglePipe<TNextInContext, NextOutContext> Append<NextOutContext>(BaseSinglePipe<TNextInContext, NextOutContext> nextPipe)
            where NextOutContext : IPipeContext;

        /// <summary>
        ///   添加网关
        /// </summary>
        /// <param name="nextPipe"></param>
        BaseBranchGateway<TNextInContext> Append(BaseBranchGateway<TNextInContext> nextPipe);
    }

}