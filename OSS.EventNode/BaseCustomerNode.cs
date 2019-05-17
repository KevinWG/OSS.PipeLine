using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.EventNode.MetaMos;
using OSS.EventNode.Mos;

namespace OSS.EventNode
{
    /// <summary>
    ///  自定义节点实现
    /// </summary>
    /// <typeparam name="TTReq"></typeparam>
    /// <typeparam name="TTRes"></typeparam>
    public abstract class BaseCustomerNode<TTReq, TTRes> : BaseNode<TTReq, TTRes> where TTReq : class
        where TTRes : ResultMo, new()
    {
        protected BaseCustomerNode():this(null)
        {
        }

        protected BaseCustomerNode(NodeMeta nodeMeta):base(nodeMeta)
        {
        }

        protected virtual Task<NodeBasicResponse<TTRes>> Proessing(TTReq req, int triedTimes)
        {
            return Task.FromResult<NodeBasicResponse<TTRes>>(null);
        }



        internal override async Task Excuting(TTReq req, NodeResponse<TTRes> nodeResp, int triedTimes, params string[] taskIds)
        {
            var cusRes = await Proessing(req, triedTimes);
            if (cusRes == null)
            {
                nodeResp.resp = new TTRes().WithResult(SysResultTypes.NoResponse, ResultTypes.ObjectNull, $"Customer Node({GetType()}) have no response!");
                nodeResp.node_status = NodeStatus.ProcessFailed;
            }
            else
            {
                nodeResp.resp = cusRes.resp;
                nodeResp.node_status = cusRes.node_status;
            }
        }


      
    }
}
