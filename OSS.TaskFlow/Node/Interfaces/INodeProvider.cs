using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.TaskFlow.Node.Mos;
using OSS.TaskFlow.Tasks;
using OSS.TaskFlow.Tasks.MetaMos;

namespace OSS.TaskFlow.Node.Interfaces
{
    public interface INodeProvider
    {
        Task<Dictionary<TaskMeta, BaseTask>> GetTaskMetas(NodeContext context);
    }


}
