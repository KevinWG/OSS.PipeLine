
namespace OSS.Pipeline.Interface
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    public interface IPipeLineEntry<in TInContext> : IPipeLine,IPipeInputExecutor<TInContext>
    {
    }
}