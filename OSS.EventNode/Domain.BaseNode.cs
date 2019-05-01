using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventNode.Mos;
using OSS.EventTask;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;

namespace OSS.EventNode
{
    /// <summary>
    ///  基础领域节点
    ///   todo 获取领域信息
    /// </summary>
    public abstract partial class BaseDomainNode<TReq, TDomain, TRes> : EventNode.BaseNode
        where TRes : ResultMo, new()
    {

        #region 入口执行方法

        /// <summary>
        ///   入口执行方法
        /// </summary>
        /// <param name="con"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<TRes> Excute(NodeContext con, TaskReqData<TReq, TDomain> req)
        {
            return (TRes) await Excute_Internal(con, req);
        }

        // 重写基类入口方法
        internal override async Task<ResultMo> Excute_Internal(NodeContext context, TaskReqData req)
        {
            // 【1】 扩展前置执行方法
            await ExcutePre(context, (TaskReqData<TReq, TDomain>) req);

            // 【2】 任务处理执行方法
            return (await base.Excute_Internal(context, req)).CheckConvertToResult<TRes>();
        }

        #endregion


        #region 生命周期扩展方法

        protected virtual Task ExcutePre(NodeContext con, TaskReqData<TReq, TDomain> req)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 内部扩展方法重写

        internal override ResultMo ExcuteResult_Internal(NodeContext context,
            Dictionary<TaskMeta, ResultMo> taskResults)
        {
            return GetNodeResult<TRes>(context, taskResults);
        }


        internal override async Task<ResultMo> GetTaskItemResult(BaseTask task, NodeContext con, TaskReqData req)
        {
            var taskContext = ConvertToContext(con, con.task_meta);
            if (task.InstanceType == InstanceType.Domain)
            {
                var domainTask = (BaseDomainTask<TReq, TDomain, TRes>) task;
                var reqData = (TaskReqData<TReq, TDomain>) req;
                return await domainTask.Process(taskContext, reqData);
            }
            else
            {
                var standTask = (BaseStandTask<TReq, TRes>) task;
                var reqData = (TaskReqData<TReq>) req;
                return await standTask.Process(taskContext, reqData);
            }
        }

        #endregion

    }
}
