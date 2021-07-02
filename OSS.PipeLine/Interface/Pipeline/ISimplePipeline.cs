namespace OSS.Pipeline.Interface
{
    /// <summary>
    ///  简单pipeline，所有管道上下文相同
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface ISimplePipeline<in TContext> : IPipeLineEntry<TContext>
    {

    }
}