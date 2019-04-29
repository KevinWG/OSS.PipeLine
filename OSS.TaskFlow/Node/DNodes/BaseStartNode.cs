using OSS.Common.ComModels;

namespace OSS.TaskFlow.Node.DNodes
{
    public class BaseStartNode<TReq, TDomain, TRes> : BaseDomainNode<TReq, TDomain, TRes>
        where TRes : ResultMo, new()
    {
    }
}
