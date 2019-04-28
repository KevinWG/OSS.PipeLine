using System.Threading.Tasks;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node.Interfaces
{
    public interface IStandNodeProvider<TReq>:INodeProvider
    {
        Task SaveTaskContext(TaskContext context, TaskReqData<TReq> data);
    }
}
