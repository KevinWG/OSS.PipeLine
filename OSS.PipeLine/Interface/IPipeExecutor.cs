using System.Threading.Tasks;

namespace OSS.Pipeline.Interface
{
   
    /// <summary>
    /// 管道对外执行接口
    /// </summary>
    public interface IPipeExecutor<in TInContext, THandleResult, TOut> : IPipe
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Task<THandleResult> Execute(TInContext para);
    }


    /// <summary>
    /// 管道对外执行接口
    /// </summary>
    public interface IPipeExecutor<in TInContext, TOut> : IPipe, IPipeExecutor<TInContext, TOut, TOut>
    {
    }

    /// <summary>
    /// 管道对外执行接口
    /// </summary>
    public interface IPipeExecutor<TOut> : IPipe, IPipeExecutor<Empty, TOut>
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <returns></returns>
        Task<TOut> Execute();
    }

    /// <summary>
    /// 管道对外执行接口
    /// </summary>
    public interface IPipeFuncExecutor<in TFuncPara, TFuncResult> : IPipe
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Task<TFuncResult> Execute(TFuncPara para);
    }
}
