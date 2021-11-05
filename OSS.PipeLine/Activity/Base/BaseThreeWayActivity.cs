using System.Threading.Tasks;
using OSS.Pipeline.Base;

namespace OSS.Pipeline.Activity.Base
{
    /// <summary>
    ///  管道执行基类（主动三向类型 ）
    ///   输入：上游传递的上下文
    ///   输出：主动结果输出， 下游上下文参数输出
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TOutContext"></typeparam>
    /// <typeparam name="THandleResult"></typeparam>
    public abstract class BaseThreeWayActivity<TInContext, THandleResult, TOutContext> : BaseThreeWayPipe<TInContext,  THandleResult, TOutContext>
    {
        /// <inheritdoc />
        protected BaseThreeWayActivity(string pipeCode,PipeType pipeType) : base(pipeCode,pipeType)
        {
        }
        
        #region 流体外部扩展

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="para">当前活动上下文信息</param>
        /// <returns>
        /// （活动是否处理成功，业务结果）
        /// traffic_signal：     
        ///     Green_Pass  - 流体自动流入后续管道
        ///     Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///     Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </returns>
        protected abstract Task<TrafficSignal<THandleResult>> Executing(TInContext para);

        #endregion
    }

}