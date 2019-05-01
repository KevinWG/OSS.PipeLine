using System.Threading.Tasks;
using OSS.EventTask.Mos;

namespace OSS.EventTask.Interfaces
{
    public interface IFlowTaskProvider<TReq,TDomain>: ITaskProvider
    {
        Task SaveTaskContext(TaskContext context, TaskReqData<TReq, TDomain> data);
    }
}
