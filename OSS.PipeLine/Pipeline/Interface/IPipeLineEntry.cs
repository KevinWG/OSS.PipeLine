
namespace OSS.Pipeline.Interface
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    public interface IPipeLineEntry<in TIn> : IPipeLine,IPipeInputExecutor<TIn>
    {
    }
}