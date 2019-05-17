using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask.Interfaces
{
    public interface IBaseTask<in TTData>
    {
        TaskMeta TaskMeta { get; }
        string ModuleName { get; set; }
        //InstanceType InstanceTaskType { get; }

        Task<bool> Revert(TTData req,  int triedTimes);
        Task<TaskResponse<ResultMo>> Run(TTData req, int triedTimes);
    }




}
