using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.EventNode.Mos;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;

namespace OSS.EventNode.Interfaces
{
    public interface INodeProvider
    {
        Task<Dictionary<TaskMeta, IBaseTask>> GetTaskMetas(NodeContext context);
    }


}
