using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask.Interfaces
{
    public interface IEventTask<in TTData>
    {
        TaskMeta TaskMeta { get; }
        string ModuleName { get; set; }
        //InstanceType InstanceTaskType { get; }

        Task<bool> Revert(TTData data,  int triedTimes);
        Task<TaskResp<ResultMo>> Run(TTData data, int triedTimes);
    }




}
