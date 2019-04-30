using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Node.Mos;
using OSS.TaskFlow.Tasks.MetaMos;
using OSS.TaskFlow.Tasks.Mos;
using OSS.TaskFlow.Tasks.Util;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  基础工作节点
    /// </summary>
    public abstract partial class BaseStandNode<TReq,TRes> : BaseNode
        where TRes : ResultMo, new()
    {
        #region 入口执行方法

        /// <summary>
        ///   入口执行方法
        /// </summary>
        /// <param name="con"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<TRes> Excute(NodeContext con, TaskReqData<TReq> req)
        {
            return (TRes)await Excute_Internal(con, req);
        }

        // 重写基类入口方法
        internal override async Task<ResultMo> Excute_Internal(NodeContext context, TaskReqData req)
        {
            // 【1】 扩展前置执行方法
            await ExcutePre(context, (TaskReqData<TReq>)req);

            // 【2】 任务处理执行方法
            return (await base.Excute_Internal(context, req)).CheckConvertToResult<TRes>();
        }

        #endregion


        #region 生命周期扩展方法

        protected virtual Task ExcutePre(NodeContext con, TaskReqData<TReq> req)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 内部扩展方法重写

        internal override ResultMo ExcuteResult_Internal(NodeContext context, Dictionary<TaskMeta, ResultMo> taskResults)
        {
            return GetNodeResult<TRes>(context, taskResults);
        }

        #endregion
    }
}
