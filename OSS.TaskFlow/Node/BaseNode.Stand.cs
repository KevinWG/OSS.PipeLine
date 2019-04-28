using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Node.Mos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  基础工作节点
    /// </summary>
    public abstract partial class BaseStandNode<TReq,TRes> :BaseNode<TReq,TRes> where TRes : ResultMo, new()
    {
        protected virtual Task ExcutePre(NodeContext con, TaskReqData<TReq> req)
        {
            return Task.CompletedTask;
        }


        internal override Task ExcutePreInternal(NodeContext con, TaskReqData<TReq> req)
        {
            return ExcutePre(con, req);
        }
    }
}
