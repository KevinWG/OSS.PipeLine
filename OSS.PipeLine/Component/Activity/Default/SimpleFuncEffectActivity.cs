using System;
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <inheritdoc />
    public class SimplePassiveEffectActivity<TPassivePara, TResult> :BasePassiveEffectActivity<TPassivePara, TResult> 
    {
        private readonly Func<TPassivePara, Task<TrafficSignal<TResult>>> _exePassive;

        /// <inheritdoc />
        public SimplePassiveEffectActivity(string pipeCode,Func<TPassivePara, Task<TrafficSignal<TResult>>> exePassive):base(pipeCode)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exePassive = exePassive ?? throw new ArgumentNullException(nameof(exePassive), "执行方法不能为空!");
        }


        /// <inheritdoc />
        protected override Task<TrafficSignal<TResult>> Executing(TPassivePara contextData)
        {
            return _exePassive(contextData);
        }
    }
}