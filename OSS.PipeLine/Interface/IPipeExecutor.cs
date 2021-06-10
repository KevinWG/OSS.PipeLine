using System.Threading.Tasks;

namespace OSS.Pipeline.Interface
{
    /// <summary>
    /// 管道对外执行接口
    /// </summary>
    public interface IPipeExecutor:IPipe
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <returns></returns>
        Task<TrafficSignal> Execute();
    }

    /// <summary>
    /// 管道对外执行接口
    /// </summary>
    public interface IPipeExecutor<in TInContext> : IPipe
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Task<TrafficSignal> Execute(TInContext para);
    }
    
    /// <summary>
    /// 管道对外执行接口
    /// </summary>
    public interface IPipeFuncExecutor<in TFuncPara, TFuncResult> : IPipe
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<TFuncResult> Execute(TFuncPara data);
    }
}
