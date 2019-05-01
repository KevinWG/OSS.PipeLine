using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.EventNode.Mos;
using OSS.EventTask.MetaMos;
using BaseTask = OSS.EventTask.BaseTask;

namespace OSS.EventNode.Interfaces
{
    public interface INodeProvider
    {
        Task<Dictionary<TaskMeta, BaseTask>> GetTaskMetas(NodeContext context);
    }


}
