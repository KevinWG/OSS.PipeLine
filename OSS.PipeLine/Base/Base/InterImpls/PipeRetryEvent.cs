using System;
using System.Threading.Tasks;
using OSS.DataFlow.Event;

namespace OSS.PipeLine.Base.Base.InterImpls
{
    /// <summary>
    ///  重试处理器
    /// </summary>
    /// <typeparam name="TPara"></typeparam>
    /// <typeparam name="TRes"></typeparam>
    internal class PipeRetryEventProcessor<TPara, TRes>
        : FlowEventProcessor<RetryEventMsg<TPara>, TRes>
    {
        public PipeRetryEventProcessor(Func<RetryEventMsg<TPara>, Task<TRes>> eventFunc, FlowEventOption option) : base(
            new PipeRetryEvent<TPara, TRes>(eventFunc), option)
        {
        }
    }

    internal class PipeRetryEvent<TPara, TRes> : IFlowEvent<RetryEventMsg<TPara>, TRes>
    {
        private readonly Func<RetryEventMsg<TPara>, Task<TRes>> _eventFunc;

        public PipeRetryEvent(Func<RetryEventMsg<TPara>, Task<TRes>> eventFunc)
        {
            _eventFunc = eventFunc;
        }

        public Task<TRes> Execute(RetryEventMsg<TPara> input)
        {
            return _eventFunc(input);
        }

        public Task Failed(RetryEventMsg<TPara> input)
        {
            return Task.CompletedTask;
        }
    }


    internal readonly struct RetryEventMsg<TPara>
    {
        public RetryEventMsg(TPara para,string prePipeCode)
        {
            pre_pipe_code = prePipeCode;
            this.para          = para;
        }

        public string pre_pipe_code { get; }

        public TPara para { get;  }
    }
}
