using System;
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <inheritdoc />
    public class SimpleFuncEffectActivity<TFuncPara, TResult> :BaseFuncEffectActivity<TFuncPara, TResult> 
    {
        private readonly Func<TFuncPara, Task<TrafficSignal<TResult>>> _exeFunc;

        /// <inheritdoc />
        public SimpleFuncEffectActivity(Func<TFuncPara, Task<TrafficSignal<TResult>>> exeFunc,string pipeCode = null)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exeFunc = exeFunc ?? throw new ArgumentNullException(nameof(exeFunc), "执行方法不能为空!");
        }


        /// <inheritdoc />
        protected override Task<TrafficSignal<TResult>> Executing(TFuncPara contextData)
        {
            return _exeFunc(contextData);
        }
    }
}