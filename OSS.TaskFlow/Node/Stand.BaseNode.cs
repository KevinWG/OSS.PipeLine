using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
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
        public  async Task<TRes> Excute(NodeContext con, TaskReqData<TReq> req)
        {
            return (TRes)await Excute_Internal(con, req);
        }
        
        internal override async Task<ResultMo> Excute_Internal(NodeContext con, TaskReqData req)
        {
            var iniRes =await InitailRunId(con);
            if (!iniRes.IsSuccess())
                return iniRes;

            return await base.Excute_Internal(con, req);
        }

        #region 重写父类扩展

        internal override Task ExcutePre_Internal(NodeContext con, TaskReqData req)
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
