using System;
using System.Threading.Tasks;
using OSS.DataFlow.Event;

namespace OSS.PipeLine.Base.Base.InterImpls
{
    public class PipeRetryEvent<TPara,TRes>:IFlowEvent<TPara, TRes>
    {
        public Task<TRes> Execute(TPara input)
        {
            throw new NotImplementedException();
        }

        public Task Failed(TPara input)
        {
            throw new NotImplementedException();
        }
    }
}
