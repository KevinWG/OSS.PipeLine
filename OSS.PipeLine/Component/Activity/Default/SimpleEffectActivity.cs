using System;
using System.Threading.Tasks;

namespace OSS.Pipeline
{


    /// <inheritdoc />
    public class SimpleEffectActivity<TResult> : BaseEffectActivity<TResult>
    {
        private readonly Func<Task<TrafficSignal<TResult>>> _exeFunc;

        /// <inheritdoc />
        public SimpleEffectActivity(Func<Task<TrafficSignal<TResult>>> exeFunc, string pipeCode=null)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exeFunc = exeFunc ?? throw new ArgumentNullException(nameof(exeFunc), "执行方法不能为空!");
        }

        /// <inheritdoc />
        protected override Task<TrafficSignal<TResult>> Executing()
        {
            return _exeFunc();
        }
    }


    /// <inheritdoc />
    public class SimpleEffectActivity<TFuncPara, TResult>: BaseEffectActivity<TFuncPara, TResult>// : BaseStraightPipe<TFuncPara, TResult>
    {
        private readonly Func<TFuncPara, Task<TrafficSignal<TResult>>> _exeFunc;

        /// <inheritdoc />
        public SimpleEffectActivity(Func<TFuncPara, Task<TrafficSignal<TResult>>> exeFunc,string pipeCode = null)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exeFunc = exeFunc ?? throw new ArgumentNullException(nameof(exeFunc), "执行方法不能为空!");
        }

        /// <inheritdoc />
        protected override Task<TrafficSignal<TResult>> Executing(TFuncPara para)
        {
            return _exeFunc(para);
        }

        ///// <inheritdoc />
        //protected override Task<(TrafficSignal traffic_signal, TResult result)> Executing(TFuncPara contextData)
        //{
        //    return _exeFunc(contextData);
        //}
    }

}