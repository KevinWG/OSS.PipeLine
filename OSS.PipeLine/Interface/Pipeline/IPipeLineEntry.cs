using System.Threading.Tasks;

namespace OSS.Pipeline.Interface
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    public interface IPipeLineEntry<in TInContext> : IPipeLine
    {
        /// <summary>
        ///  启动执行方法
        /// </summary>
        /// <returns></returns>
        Task<TrafficResult> Execute(TInContext context);
    }
}