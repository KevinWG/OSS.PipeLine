using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Tasks.Interfaces;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tasks
{
    public abstract partial class BaseDomainTask<TReq,TDomain, TRes> : BaseTask
        where TRes : ResultMo, new()
        where TDomain : IDomainMo
    {
        #region 具体任务执行入口

        /// <summary>
        ///   任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns>  </returns>
        public async Task<TRes> Process(TaskContext context, TaskReqData<TReq,TDomain> data)
        {
            return (await base.Process(context, data)) as TRes;
        }
        
        #endregion

        #region 实现，重试，失败 执行  重写父类方法

        internal override async Task<ResultMo> Do_Internal(TaskContext context, TaskReqData data)
        {
            return await Do(context,(TaskReqData<TReq, TDomain>)data);
        }

        internal override Task Failed_Internal(TaskContext context, TaskReqData data)
        {
            return Failed(context, (TaskReqData<TReq, TDomain>)data);
        }

        internal override Task Revert_Internal(TaskContext context, TaskReqData data)
        {
            return Revert(context, (TaskReqData<TReq, TDomain>)data);
        }

        internal override Task ProcessEnd_Internal(ResultMo taskRes, TaskReqData data, TaskContext context)
        {
            return ProcessEnd((TRes)taskRes, (TaskReqData<TReq, TDomain>)data, context);
        }
        #endregion

        #region 实现，重试，失败 执行  扩展方法

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns> sys_ret = (int)SysResultTypes.RunFailed 系统会字段判断是否满足重试条件执行重试    </returns>
        protected abstract Task<TRes> Do(TaskContext context, TaskReqData<TReq, TDomain> data);

        /// <summary>
        ///  执行失败回退操作
        ///   如果设置了重试配置，调用后重试
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        protected internal virtual Task Revert(TaskContext context, TaskReqData<TReq, TDomain> data)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///  最终失败执行方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        protected virtual Task Failed(TaskContext context, TaskReqData<TReq, TDomain> data)
        {
            return Task.CompletedTask;
        }


        /// <summary>
        /// 执行结束方法
        /// </summary>
        /// <param name="taskRes">任务结果 :
        ///  sys_ret = (int)SysResultTypes.RunFailed表明最终执行失败，
        ///  sys_ret = (int)SysResultTypes.RunPause表示符合间隔重试条件，会通过 contextKeeper 保存信息后续唤起
        /// </param>
        /// <param name="data">请求的数据信息</param>
        /// <param name="context">请求的上线文</param>
        /// <returns></returns>
        protected virtual Task ProcessEnd(TRes taskRes, TaskReqData<TReq,TDomain> data, TaskContext context)
        {
            return Task.CompletedTask;
        }


        #endregion
    }
}