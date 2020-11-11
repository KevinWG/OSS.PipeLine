using System.Threading.Tasks;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask.Interfaces
{
    public interface IEventTask<in TTData, TTRes> : IBaseEventTask<TTData, TTRes>
        //where TTRes : class, new()
    {
        Task<bool> Revert(TTData data);
    }

    public interface IBaseEventTask<in TTData, TTRes>: IMeta<EventTaskMeta>
        //where TTRes :class, new()
    {
      
        Task<EventTaskResp<TTRes>> Process(TTData data);
        Task<EventTaskResp<TTRes>> Process(TTData data,int triedTimes);
    }




}
