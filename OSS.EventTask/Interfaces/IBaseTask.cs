using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask.Interfaces
{
    public interface IBaseTask<in TTReq>
    {
        TaskMeta TaskMeta { get; }
        string ModuleName { get; set; }
        InstanceType InstanceTaskType { get; }

        Task<bool> Revert(TTReq req);
        Task<TaskResponse<ResultMo>> Run(TTReq req, RunCondition runCondition);
    }
}
