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
        public Task<TRes> Excute(NodeContext con, TaskReqData<TReq, TFlowData> req)
        {
            return Excute(con, req);
        }


        protected virtual Task ExcutePre(NodeContext con, TaskReqData<TReq, TFlowData> req)
        {
            return Task.CompletedTask;
        }
        
        internal override Task ExcutePreInternal(NodeContext con, TaskReqData<TReq> req)
        {
            return ExcutePre(con,(TaskReqData<TReq, TFlowData>)req);
        }
    }
}
