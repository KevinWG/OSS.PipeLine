using System.Threading.Tasks;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask.Interfaces
{
    public interface IEventTask<in TTData>: IMeta<TaskMeta>
    {
        Task<bool> Revert(TTData data);
    }

    public interface IEventTask<in TTData, TTRes>: IEventTask<TTData>
        where TTRes :class, new()
    {
      
        Task<TaskResp<TTRes>> Process(TTData data, int triedTimes);
    }




}
