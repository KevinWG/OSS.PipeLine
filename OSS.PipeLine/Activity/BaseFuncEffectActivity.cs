using System.Threading.Tasks;
using OSS.Pipeline.Interface;
using OSS.Pipeline.Base;
using OSS.Pipeline.Activity.Base;

namespace OSS.Pipeline
{
    /// <summary>
    ///  被动触发执行活动组件基类
    ///      传入TFuncPara类型参数，自身返回处理结果，且结果作为上下文传递给下一个节点
    /// </summary>
    /// <typeparam name="TFuncPara"></typeparam>
    /// <typeparam name="TFuncResult"></typeparam>
    public abstract class BaseFuncEffectActivity<TFuncPara, TFuncResult> :
        BaseThreeWayFuncActivity<TFuncPara, TFuncResult, TFuncResult>, IFuncEffectActivity<TFuncPara, TFuncResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseFuncEffectActivity() : base(PipeType.FuncEffectActivity)
        {
        }
        
        internal override async Task<TrafficResult<TFuncResult, TFuncResult>> InterProcessPackage(TFuncPara context)
        {
            var tSignal = await Executing(context);
            return new TrafficResult<TFuncResult, TFuncResult>(tSignal,
                tSignal.signal == SignalFlag.Red_Block ? PipeCode : string.Empty, tSignal.result);
        }
    }
}