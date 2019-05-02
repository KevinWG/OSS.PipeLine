using System.Threading.Tasks;
using OSS.EventTask.Mos;

namespace OSS.EventTask.Interfaces
{
    public interface IStandTaskProvider<TReq>: ITaskProvider
    {
        Task SaveTaskContext(TaskContext<TReq> context);
    }
}
