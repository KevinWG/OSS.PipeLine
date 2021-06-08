using System;
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <inheritdoc />
    internal class InterEffectActivity<TFuncPara, TResult>: BaseEffectActivity<TFuncPara, TResult>// : BaseStraightPipe<TFuncPara, TResult>
    {
        private readonly Func<TFuncPara,Task<(bool is_ok, TResult result)>> _exeFunc;

        /// <inheritdoc />
        public InterEffectActivity(Func<TFuncPara,Task<(bool is_ok, TResult result)>> exeFunc,string pipeCode)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exeFunc = exeFunc ?? throw new ArgumentNullException(nameof(exeFunc), "执行方法不能为空!");
        }

        /// <inheritdoc />
        protected override Task<(bool is_ok, TResult result)> Executing(TFuncPara contextData)
        {
            return _exeFunc(contextData);
        }
    }

    /// <inheritdoc />
    internal class InterEffectActivity<TResult> : BaseEffectActivity<TResult>
    {
        private readonly Func< Task<(bool is_ok, TResult result)>> _exeFunc;

        /// <inheritdoc />
        public InterEffectActivity(Func< Task<(bool is_ok, TResult result)>> exeFunc,string pipeCode)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exeFunc = exeFunc ?? throw new ArgumentNullException(nameof(exeFunc), "执行方法不能为空!");
        }

        /// <inheritdoc />
        protected override Task<(bool is_ok, TResult result)> Executing()
        {
            return _exeFunc();
        }
    }
}