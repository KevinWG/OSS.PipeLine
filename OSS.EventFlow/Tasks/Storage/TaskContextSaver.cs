using OSS.EventFlow.Tasks.Mos;
using System.Threading.Tasks;

namespace OSS.EventFlow.Tasks.Storage
{
    public interface ITaskContextSaver<TPara>
    {
        Task SaveTaskContext(TaskContext context, TPara reqPara);
    }
}
