using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventNode.MetaMos;
using OSS.EventNode.Mos;

namespace OSS.EventNode.Interfaces
{
    public interface IEventNode
    {
        NodeMeta NodeMeta { get; }
    }

    public interface IEventNode<TTData, TTRes>: IEventNode
        where TTData : class
        where TTRes : ResultMo, new()
    {
        Task<NodeResp<TTRes>> Process(TTData data);

        Task<NodeResp<TTRes>> Process(TTData data, int triedTimes, params string[] taskIds);
    }

}
