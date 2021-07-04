using OSS.Pipeline.InterImpls.Watcher;

namespace OSS.Pipeline.Interface
{
    /// <summary>
    ///  管道基础接口
    /// </summary>
    public interface IPipeLine : IPipe
    {
        /// <summary>
        ///  开始管道
        /// </summary>
        IPipe StartPipe { get; }
        
        /// <summary>
        ///  结束管道
        /// </summary>
        IPipe EndPipe { get; }

        /// <summary>
        ///   获取路由
        /// </summary>
        /// <returns></returns>
        PipeRoute ToRoute();
        
        internal PipeWatcherProxy GetProxy();
    }

    /// <summary>
    ///  Pipeline管道
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public interface IPipeLine<in TIn, TOut> : IPipeLine, IPipeLineEntry<TIn>
    {
    }

}