using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Interface
{
    /// <summary>
    ///  管道链接器
    /// </summary>
    /// <typeparam name="TNextInContext"></typeparam>
    public interface INextPipeAppender<TNextInContext>
        where TNextInContext : FlowContext
    {
        /// <summary>
        ///  追加管道
        /// </summary>
        /// <param name="nextPipe"></param>
        void Append(BasePipe<TNextInContext> nextPipe);
    }

}