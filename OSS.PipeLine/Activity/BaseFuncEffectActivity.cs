using System.Threading.Tasks;
using OSS.Pipeline.Interface;
using OSS.Pipeline.Base;
using OSS.Pipeline.InterImpls.Watcher;

namespace OSS.Pipeline
{
    /// <summary>
    ///  被动触发执行活动组件基类
    ///      传入TFuncPara类型参数，自身返回处理结果，且结果作为上下文传递给下一个节点
    /// </summary>
    /// <typeparam name="TFuncPara"></typeparam>
    /// <typeparam name="TFuncResult"></typeparam>
    public abstract class BaseFuncEffectActivity<TFuncPara, TFuncResult> :
        BaseFuncPipe<TFuncPara, TFuncResult>, IFuncEffectActivity<TFuncPara, TFuncResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseFuncEffectActivity() : base(PipeType.FuncEffectActivity)
        {
        }

        /// <summary>
        ///  执行方法
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public async Task<TFuncResult> Execute(TFuncPara para)
        {
            var trafficSignal = await Executing(para);
            var trafficRes = new TrafficResult(trafficSignal.signal,trafficSignal.result,
                trafficSignal.signal == SignalFlag.Red_Block ? PipeCode : string.Empty, trafficSignal.msg);

            await Watch(PipeCode, PipeType, WatchActionType.Executed, para, trafficRes);
            if (trafficSignal.signal == SignalFlag.Green_Pass)
            {
                await ToNextThrough(trafficSignal.result);
            }

            if (trafficSignal.signal == SignalFlag.Red_Block)
            {
                await InterBlock(para, trafficRes);
            }
         
            return trafficSignal.result;
        }

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="para">当前活动上下文信息</param>
        /// <returns>
        /// (bool traffic_signal,TResult result)-（活动是否处理成功，业务结果）
        /// traffic_signal：     
        ///     Green_Pass  - 流体自动流入后续管道
        ///     Yellow_Wait - 暂停执行，既不向后流动，也不触发Block。
        ///     Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </returns>
        protected abstract Task<TrafficSignal<TFuncResult>> Executing(TFuncPara para);
    }
}