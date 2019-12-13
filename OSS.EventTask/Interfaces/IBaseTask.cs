using System.Threading.Tasks;
using OSS.Common.Resp;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask.Interfaces
{
    public interface IEventTask<in TTData>
    {
        TaskMeta TaskMeta { get; }
   
        Task<bool> Revert(TTData data,  int triedTimes);
        Task<TaskResp<Resp>> Run(TTData data, int triedTimes);
    }




}
