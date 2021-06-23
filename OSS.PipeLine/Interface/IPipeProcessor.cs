using System.Threading.Tasks;

namespace OSS.Pipeline.Interface
{
    ///// <summary>
    /////  管道处理器
    ///// </summary>
    ///// <typeparam name="TInContext"></typeparam>
    //internal interface IPipeProcessor<in TInContext>
    //{
    //    /// <summary>
    //    ///  内部管道 -- (1) 上游管道调用接口  
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <returns></returns>
    //    Task<TrafficResult> InterPreCall(TInContext context);

    //}
    
    ///// <summary>
    /////  管道处理器
    ///// </summary>
    ///// <typeparam name="TInContext"></typeparam>
    ///// <typeparam name="THandlePara"></typeparam>
    ///// <typeparam name="THandleResult"></typeparam>
    ///// <typeparam name="TOutContext"></typeparam>
    //internal interface IPipeProcessor<in TInContext, THandlePara, THandleResult, TOutContext> : IPipeProcessor<TInContext>
    //{
    //    /// <summary>
    //    ///  内部管道 -- （2）执行
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <returns></returns>
    //    internal Task<TrafficResult<THandleResult, TOutContext>> InterProcess(THandlePara context);

    //    /// <summary>
    //    ///  内部管道 -- （3）执行 - 控制流转，阻塞处理
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <returns></returns>
    //    internal Task<TrafficResult<THandleResult, TOutContext>> InterProcessHandling(THandlePara context);

    //    /// <summary>
    //    ///  内部管道 -- （4）执行 - 组装业务处理结果
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <returns></returns>
    //    internal Task<TrafficResult<THandleResult, TOutContext>> InterProcessPackage(THandlePara context);
    //}
}