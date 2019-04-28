using System.Threading.Tasks;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node.Interfaces
{
    public interface IFlowNodeProvider<TReq,TFlowData>:INodeProvider
    {
        Task SaveTaskContext(TaskContext context, TaskReqData<TReq, TFlowData> data);
    }
}
