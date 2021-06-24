using System.Threading.Tasks;
using OSS.Pipeline.Interface;
using OSS.Pipeline.InterImpls.Watcher;

namespace OSS.Pipeline.Base
{
    /// <summary>
    ///  管道执行基类（主动三向类型 ）
    ///   输入：上游传递的上下文
    ///   输出：主动结果输出， 下游上下文参数输出
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TOutContext"></typeparam>
    /// <typeparam name="THandleResult"></typeparam>
    public abstract class BaseThreeWayPipe<TInContext, THandleResult, TOutContext> : BaseFourWayPipe<TInContext, TInContext, THandleResult, TOutContext>,
        IPipeExecutor<TInContext, THandleResult, TOutContext>
    {
        /// <inheritdoc />
        protected BaseThreeWayPipe(PipeType pipeType) : base(pipeType)
        {
        }

        #region 流体外部扩展

        /// <summary>
        /// 外部执行方法 - 启动入口
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public async Task<THandleResult> Execute(TInContext para)
        {
            return (await InterProcess(para)).result;
        }
        
        #endregion


        #region 流体内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficResult> InterPreCall(TInContext context)
        {
            await Watch(PipeCode, PipeType, WatchActionType.Starting, context);
            var tRes = await InterProcess(context);
            return tRes.ToResult();
        }

        #endregion
    }
}