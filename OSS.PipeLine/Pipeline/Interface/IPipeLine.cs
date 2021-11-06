using OSS.Pipeline.InterImpls.Watcher;

namespace OSS.Pipeline.Interface
{
    /// <summary>
    ///  管道基础接口
    /// </summary>
    public interface IPipeLine : IPipeMeta
    {
        /// <summary>
        ///  开始管道
        /// </summary>
        IPipeMeta StartPipe { get; }
        
        /// <summary>
        ///  结束管道
        /// </summary>
        IPipeMeta EndPipe { get; }

        /// <summary>
        ///   获取路由
        /// </summary>
        /// <returns></returns>
        PipeRoute ToRoute();
        
        /// <summary>
        /// 获取内部监控代理器
        /// </summary>
        /// <returns></returns>
        internal PipeWatcherProxy GetWatchProxy();
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