using OSS.Pipeline.Interface;

namespace OSS.Pipeline.SimplePipeline.Interface
{
    /// <summary>
    ///  简单Pipeline管道
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface ISimplePipeline<TContext>:IPipeLine<TContext,TContext>
    {
    }
}
