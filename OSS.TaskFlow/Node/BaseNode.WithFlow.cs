using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Node.Mos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  基础工作节点
    /// </summary>
    public abstract partial class BaseFlowNode<TReq, TFlowData, TRes> : BaseNode<TReq, TRes> where TRes : ResultMo, new()
    {
        public  Task<ResultMo> Excute(NodeContext con, TaskReqData<TReq, TFlowData> req)
        {
            return Excute(con, req);
        }
    }
}
