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
    /// <typeparam name="TTData"></typeparam>
    /// <typeparam name="TTRes"></typeparam>
    public abstract class BaseCustomerNode<TTData, TTRes> : BaseNode<TTData, TTRes> where TTData : class
        where TTRes : ResultMo, new()
    {
        protected BaseCustomerNode():this(null)
        {
        }

        protected BaseCustomerNode(NodeMeta nodeMeta):base(nodeMeta)
        {
        }

        protected virtual Task<NodeBasicResponse<TTRes>> Proessing(TTData data, int triedTimes)
        {
            return Task.FromResult<NodeBasicResponse<TTRes>>(null);
        }
        
        internal override async Task Excuting(TTData data, NodeResponse<TTRes> nodeResp, int triedTimes, params string[] taskIds)
        {
            var cusRes = await Proessing(data, triedTimes);
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
