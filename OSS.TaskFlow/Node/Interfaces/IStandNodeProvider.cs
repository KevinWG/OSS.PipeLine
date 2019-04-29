using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Node.Mos;

namespace OSS.TaskFlow.Node.Interfaces
{
    public interface IStandNodeProvider<TReq> : INodeProvider
    {
        Task<ResultIdMo> GenerateRunId(NodeContext context);
    }
}
