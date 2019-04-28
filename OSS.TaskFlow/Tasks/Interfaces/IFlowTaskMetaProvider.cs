using System.Threading.Tasks;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tasks.Interfaces
{
    public interface IFlowTaskMetaProvider<TReq,TFlowData>:ITaskMetaProvider
    {
        Task SaveTaskContext(TaskContext context, TaskReqData<TReq, TFlowData> data);
    }
}
