using System;
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <inheritdoc />
    public class SimpleFuncEffectActivity<TFuncPara, TResult> :BaseFuncEffectActivity<TFuncPara, TResult> 
    {
        private readonly Func<TFuncPara, Task<(TrafficSingleValue tsValue, TResult result)>> _exeFunc;

        /// <inheritdoc />
        public SimpleFuncEffectActivity(Func<TFuncPara, Task<(TrafficSingleValue tsValue, TResult result)>> exeFunc,string pipeCode = null)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exeFunc = exeFunc ?? throw new ArgumentNullException(nameof(exeFunc), "执行方法不能为空!");
        }


        protected override Task<(TrafficSingleValue tsValue, TResult result)> Executing(TFuncPara contextData)
        {
            return _exeFunc(contextData);
        }
    }
}