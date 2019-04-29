using System.Threading.Tasks;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tasks.Interfaces
{
    public interface IFlowTaskProvider<TReq,TDomain>: ITaskProvider
    {
        Task SaveTaskContext(TaskContext context, TaskReqData<TReq, TDomain> data);
    }
}
