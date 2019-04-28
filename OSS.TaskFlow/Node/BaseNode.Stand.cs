using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Node.Mos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  基础工作节点
    /// </summary>
    public abstract partial class BaseStandNode<TReq,TRes> : BaseNode<TRes>
        where TRes : ResultMo, new()
    {
        /// <summary>
        ///  执行入口方法
        /// </summary>
        /// <param name="con"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<TRes> Excute(NodeContext con, TaskReqData<TReq> req)
        {
            return await base.Excute(con, req) as TRes;
        }


        #region 重写父类扩展

        internal override Task ExcutePreInternal(NodeContext con, TaskReqData req)
        {
            return ExcutePre(con, (TaskReqData<TReq>)req);
        }

        #endregion


        #region 对外扩展方法

        protected virtual Task ExcutePre(NodeContext con, TaskReqData<TReq> req)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
