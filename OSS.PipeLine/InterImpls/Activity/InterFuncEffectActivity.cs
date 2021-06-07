using System;
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <inheritdoc />
    internal class InterFuncEffectActivity<TFuncPara, TFuncResult> :BaseFuncEffectActivity<TFuncPara, TFuncResult> 
    {
        private readonly Func<TFuncPara, Task<(bool is_ok, TFuncResult result)>> _exeFunc;

        /// <inheritdoc />
        public InterFuncEffectActivity(Func<TFuncPara, Task<(bool is_ok, TFuncResult result)>> exeFunc)
        {
            _exeFunc = exeFunc ?? throw new ArgumentNullException(nameof(exeFunc), "执行方法不能为空!");
        }


        protected override Task<(bool is_ok, TFuncResult result)> Executing(TFuncPara contextData)
        {
            return _exeFunc(contextData);
        }
    }
}