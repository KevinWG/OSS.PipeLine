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

        #region  进入-执行-返回 -   对外扩展方法


        public Task Activate(NodeContext context)
        {
            return MoveIn(context);
        }


        /// <summary>
        ///  前置进入方法
        /// </summary>
        /// <returns></returns>
        protected internal virtual Task MoveIn(NodeContext con)
        {
            return Task.CompletedTask;
        }

        #endregion



        #region 执行 -- 对外扩展方法
         
        protected virtual Task ExcutePre(NodeContext con, TaskReqData<TReq, TFlowData> req)
        {
            return Task.CompletedTask;
        }


        #endregion


        #region 重写父类扩展

        internal override Task ExcutePre_Internal(NodeContext con, TaskReqData req)
        {
            return ExcutePre(con,(TaskReqData<TReq, TFlowData>)req);
        }  
        #endregion
    }
}
