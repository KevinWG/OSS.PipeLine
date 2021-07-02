
using System.Threading.Tasks;

namespace OSS.Pipeline.Interface
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    public interface IPipeLineEntry<in TInContext> : IPipeLine//,IPipeInputExecutor<TInContext>
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <returns></returns>
        Task Execute(TInContext para);
    }
}