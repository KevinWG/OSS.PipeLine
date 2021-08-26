using System;
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <inheritdoc />
    public class SimplePassiveEffectActivity<TPassivePara, TResult> :BasePassiveEffectActivity<TPassivePara, TResult> 
    {
        private readonly Func<TPassivePara, Task<TrafficSignal<TResult>>> _exeFunc;

        /// <inheritdoc />
        public SimplePassiveEffectActivity(string pipeCode,Func<TPassivePara, Task<TrafficSignal<TResult>>> exeFunc):base(pipeCode)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exeFunc = exeFunc ?? throw new ArgumentNullException(nameof(exeFunc), "执行方法不能为空!");
        }


        /// <inheritdoc />
        protected override Task<TrafficSignal<TResult>> Executing(TPassivePara contextData)
        {
            return _exeFunc(contextData);
        }
    }
}