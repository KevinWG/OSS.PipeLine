using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tasks
{
    public abstract partial class BaseFlowTask<TReq,TFlowData, TRes> : BaseTask<TReq>
        where TRes : ResultMo, new()
    {
        #region 具体任务执行入口

        /// <summary>
        ///   任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns>  </returns>
        public async Task<TRes> Process(TaskContext context, TaskReqData<TReq,TFlowData> data)
        {
            return (await base.ProcessInternal(context, data)) as TRes;
        }
        
        #endregion

        #region 实现，重试，失败 执行  重写父类方法

        internal override async Task<ResultMo> Do_Internal(TaskContext context, TaskReqData<TReq> data)
        {
            return await Do(context,(TaskReqData<TReq, TFlowData>)data);
        }

        internal override Task Failed_Internal(TaskContext context, TaskReqData<TReq> data)
        {
            return Failed(context, (TaskReqData<TReq, TFlowData>)data);
        }

        internal override Task Revert_Internal(TaskContext context, TaskReqData<TReq> data)
        {
            return Revert(context, (TaskReqData<TReq, TFlowData>)data);
        }

        internal override Task SaveTaskContext(TaskContext context, TaskReqData<TReq> data)
        {
            return _contextKepper.Invoke(context, (TaskReqData<TReq, TFlowData>)data);
        }


        internal override Task ProcessEnd_Internal(ResultMo taskRes, TaskReqData<TReq> data, TaskContext context)
        {
            return ProcessEnd((TRes)taskRes, (TaskReqData<TReq, TFlowData>)data, context);
        }
        #endregion

        #region 实现，重试，失败 执行  扩展方法

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns>  特殊：ret=-100（EventFlowResult.Failed）任务处理失败，执行回退，并根据重试设置发起重试</returns>
        protected abstract Task<TRes> Do(TaskContext context, TaskReqData<TReq, TFlowData> data);

        /// <summary>
        ///  执行失败回退操作
        ///   如果设置了重试配置，调用后重试
        /// </summary>
        /// <param name="context"></param>
        protected internal virtual Task Revert(TaskContext context, TaskReqData<TReq, TFlowData> data)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///  最终失败执行方法
        /// </summary>
        /// <param name="context"></param>
        protected virtual Task Failed(TaskContext context, TaskReqData<TReq, TFlowData> data)
        {
            return Task.CompletedTask;
        }


        /// <summary>
        /// 执行结束方法
        /// </summary>
        /// <param name="taskRes"></param>
        /// <param name="data"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task ProcessEnd(TRes taskRes, TaskReqData<TReq,TFlowData> data, TaskContext context)
        {
            return Task.CompletedTask;
        }


        #endregion
    }
}