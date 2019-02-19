using OSS.Common.ComModels;
using OSS.EventFlow.Tasks.Mos;

namespace OSS.EventFlow.Tasks.Interfaces
{
    public interface ITaskRetryProducer<TPara>
    {
        void Save(TaskOption config, TaskReq<TPara> req);
    }
}
