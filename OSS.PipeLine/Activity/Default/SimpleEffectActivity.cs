using System;
using System.Threading.Tasks;

namespace OSS.Pipeline
{


    /// <inheritdoc />
    public class SimpleEffectActivity<TResult> : BaseEffectActivity<TResult>
    {
        private readonly Func<Task<TrafficSignal<TResult>>> _exeFunc;

        /// <inheritdoc />
        public SimpleEffectActivity(string pipeCode, Func<Task<TrafficSignal<TResult>>> exeFunc):base(pipeCode)
        {
            _exeFunc = exeFunc ?? throw new ArgumentNullException(nameof(exeFunc), "执行方法不能为空!");
        }

        /// <inheritdoc />
        protected override Task<TrafficSignal<TResult>> Executing()
        {
            return _exeFunc();
        }
    }


    /// <inheritdoc />
    public class SimpleEffectActivity<TPassivePara, TResult>: BaseEffectActivity<TPassivePara, TResult>// : BaseStraightPipe<TPassivePara, TResult>
    {
        private readonly Func<TPassivePara, Task<TrafficSignal<TResult>>> _exeFunc;

        /// <inheritdoc />
        public SimpleEffectActivity( string pipeCode,Func<TPassivePara, Task<TrafficSignal<TResult>>> exeFunc):base(pipeCode)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exeFunc = exeFunc ?? throw new ArgumentNullException(nameof(exeFunc), "执行方法不能为空!");
        }

        /// <inheritdoc />
        protected override Task<TrafficSignal<TResult>> Executing(TPassivePara para)
        {
            return _exeFunc(para);
        }

        ///// <inheritdoc />
        //protected override Task<(TrafficSignal traffic_signal, TResult result)> Executing(TPassivePara contextData)
        //{
        //    return _exeFunc(contextData);
        //}
    }

}