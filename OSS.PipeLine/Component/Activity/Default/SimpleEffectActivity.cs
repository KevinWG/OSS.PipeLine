using System;
using System.Threading.Tasks;

namespace OSS.Pipeline
{


    /// <inheritdoc />
    public class SimpleEffectActivity<TResult> : BaseEffectActivity<TResult>
    {
        private readonly Func<Task<TrafficSignal<TResult>>> _exePassive;

        /// <inheritdoc />
        public SimpleEffectActivity(string pipeCode, Func<Task<TrafficSignal<TResult>>> exePassive):base(pipeCode)
        {
            _exePassive = exePassive ?? throw new ArgumentNullException(nameof(exePassive), "执行方法不能为空!");
        }

        /// <inheritdoc />
        protected override Task<TrafficSignal<TResult>> Executing()
        {
            return _exePassive();
        }
    }


    /// <inheritdoc />
    public class SimpleEffectActivity<TPassivePara, TResult>: BaseEffectActivity<TPassivePara, TResult>// : BaseStraightPipe<TPassivePara, TResult>
    {
        private readonly Func<TPassivePara, Task<TrafficSignal<TResult>>> _exePassive;

        /// <inheritdoc />
        public SimpleEffectActivity( string pipeCode,Func<TPassivePara, Task<TrafficSignal<TResult>>> exePassive):base(pipeCode)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exePassive = exePassive ?? throw new ArgumentNullException(nameof(exePassive), "执行方法不能为空!");
        }

        /// <inheritdoc />
        protected override Task<TrafficSignal<TResult>> Executing(TPassivePara para)
        {
            return _exePassive(para);
        }

        ///// <inheritdoc />
        //protected override Task<(TrafficSignal traffic_signal, TResult result)> Executing(TPassivePara contextData)
        //{
        //    return _exePassive(contextData);
        //}
    }

}