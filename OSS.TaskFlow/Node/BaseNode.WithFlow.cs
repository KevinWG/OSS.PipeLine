using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Node.Mos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  基础工作节点
    /// </summary>
    public abstract partial class BaseFlowNode<TReq, TFlowData, TRes> : BaseNode<TRes>
        where TRes : ResultMo, new()
    {

        /// <summary>
        ///   主执行方法
        /// </summary>
        /// <param name="con"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<TRes> Excute(NodeContext con, TaskReqData<TReq, TFlowData> req)
        {
            return Excute(con, req);
        }



        #region 对外扩展方法

        protected virtual Task ExcutePre(NodeContext con, TaskReqData<TReq, TFlowData> req)
        {
            return Task.CompletedTask;
        }


        #endregion


        #region 重写父类扩展

        internal override Task ExcutePreInternal(NodeContext con, TaskReqData req)
        {
            return ExcutePre(con,(TaskReqData<TReq, TFlowData>)req);
        }  
        #endregion
    }
}
