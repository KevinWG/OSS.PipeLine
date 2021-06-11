using OSS.Pipeline.InterImpls.Watcher;
using System.Threading.Tasks;

namespace OSS.Pipeline.Base
{
    /// <summary>
    ///  管道基类（空参被动类型）
    /// </summary>
    /// <typeparam name="TFuncResult"></typeparam>
    /// <typeparam name="TFuncPara"></typeparam>
    public abstract class BaseFuncPipe<TFuncPara, TFuncResult> :
       BasePipe<EmptyContext, TFuncPara, TFuncResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseFuncPipe(PipeType pipeType) : base(pipeType)
        {
        }
        
        #region 内部的业务处理

        internal override async Task<TrafficResult> InterStart(EmptyContext context)
        {
            await Watch(PipeCode, PipeType, WatchActionType.Starting, context);
            return new TrafficResult(TrafficSignal.Green_Pass);
        }

        #endregion

    }
}
