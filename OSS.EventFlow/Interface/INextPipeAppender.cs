using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Interface
{
    /// <summary>
    ///  管道链接器
    /// </summary>
    /// <typeparam name="TNextInContext"></typeparam>
    public interface IPipeAppender<TNextInContext>
        where TNextInContext : FlowContext
    {
        /// <summary>
        ///  追加管道
        /// </summary>
        /// <param name="nextPipe"></param>
        void Append(BasePipe<TNextInContext> nextPipe);

        /// <summary>
        /// 追加管道
        /// </summary>
        /// <param name="nextPipe"></param>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <returns>下个管道的追加器</returns>
        IPipeAppender<NextOutContext> Append<NextOutContext>(BaseSinglePipe<TNextInContext, NextOutContext> nextPipe)
            where NextOutContext : FlowContext;
    }

}