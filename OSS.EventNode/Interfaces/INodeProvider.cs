using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.EventTask.Interfaces;

namespace OSS.EventNode.Interfaces
{
    public interface INodeProvider<TTReq>
    {
        Task<IList<IBaseTask<TTReq>>> GetTaskMetas();
    }


}
