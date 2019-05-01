using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;

namespace OSS.EventTask
{
    public abstract partial class BaseStandTask<TReq, TRes> : EventTask.BaseTask
        where TRes : ResultMo, new()
    {
        #region 具体任务执行入口

        /// <summary>
        ///   任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns>  </returns>
        public async Task<TRes> Process(TaskContext context, TaskReqData<TReq> data)
        {
            return (TRes)(await Process_Internal(context, data));
        }

        internal override async Task<ResultMo> Process_Internal(TaskContext context, TaskReqData data)
        {
            CheckTaskContext(context);
            // 【1】 执行起始方法
            await ProcessStart(context, (TaskReqData<TReq>)data);
            // 【2】  执行核心方法
            var res = (await base.Process_Internal(context, data)).CheckConvertToResult<TRes>();
            // 【3】 执行结束方法
            await ProcessEnd(res, context, (TaskReqData<TReq>)data);
            return res;
        }
        #endregion

        #region 生命周期扩展方法

        /// <summary>
        /// 任务开始方法
        /// </summary>
        /// <param name="context">请求的上下文</param>
        /// <param name="data">请求的数据信息</param>
        /// <returns></returns>
        protected virtual Task ProcessStart(TaskContext context, TaskReqData<TReq> data)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 任务结束方法
        /// </summary>
        /// <param name="taskRes">任务结果 :
        ///  sys_ret = (int)SysResultTypes.RunFailed表明最终执行失败，
        ///  sys_ret = (int)SysResultTypes.RunPause表示符合间隔重试条件，会通过 contextKeeper 保存信息后续唤起
        /// </param>
        /// <param name="context">请求的上下文</param>
        /// <param name="data">请求的数据信息</param>
        /// <returns></returns>
        protected virtual Task ProcessEnd(TRes taskRes, TaskContext context, TaskReqData<TReq> data)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 扩展方法（实现，回退，失败）  扩展方法

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns> sys_ret = (int)SysResultTypes.RunFailed 系统会字段判断是否满足重试条件执行重试    </returns>
        protected abstract Task<TRes> Do(TaskContext context, TaskReqData<TReq> data);

        /// <summary>
        ///  执行失败回退操作
        ///   如果设置了重试配置，调用后重试
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        protected internal virtual Task Revert(TaskContext context, TaskReqData<TReq> data)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///  最终失败执行方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        protected virtual Task Failed(TaskContext context, TaskReqData<TReq> data)
        {
            return Task.CompletedTask;
        }
        #endregion

        #region 基础内部扩展方法（实现，回退，失败） 重写

        internal override async Task<ResultMo> Do_Internal(TaskContext context, TaskReqData data)
        {
            return await Do(context,  (TaskReqData<TReq>)data);
        }

        internal override Task Failed_Internal(TaskContext context, TaskReqData data)
        {
            return Failed(context, (TaskReqData<TReq>)data);
        }

        internal override Task Revert_Internal(TaskContext context, TaskReqData data)
        {
            return Revert(context, (TaskReqData<TReq>)data);
        }

        #endregion

       
    }
}