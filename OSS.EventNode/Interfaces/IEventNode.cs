using System.Threading.Tasks;
using OSS.EventNode.MetaMos;
using OSS.EventNode.Mos;
using OSS.EventTask.MetaMos;

namespace OSS.EventNode.Interfaces
{
    public interface IEventNode:IMeta<NodeMeta>
    {
    }

    public interface IEventNode<TTData, TTRes>: IEventNode
        where TTData : class
        where TTRes : class, new()
    {
        Task<NodeResp<TTRes>> Process(TTData data);

        Task<NodeResp<TTRes>> Process(TTData data, int triedTimes, params string[] taskIds);
    }

}
