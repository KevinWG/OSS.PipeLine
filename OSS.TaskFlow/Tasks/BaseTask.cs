using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tasks
{
    public abstract partial class  BaseTask
    {
        #region 具体任务执行入口

        /// <summary> 
        ///   任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns>  </returns>
        internal async Task<ResultMo> Process(TaskContext context, TaskReqData data)
        {
            var checkRes = context.CheckTaskContext();
            if (!checkRes.IsSuccess())
                return checkRes;
            
            var res = await Recurs(context, data);

            // 判断是否间隔执行,生成重试信息
            if (res.IsTaskFailed() && context.interval_times < context.task_meta.interval_times)
            {
                context.interval_times++;
                await SaveTaskContext_Internal(context, data);
                res.sys_ret = (int) SysResultTypes.RunPause; // TaskResultType.WatingActivation;
            }

            if (res.IsTaskFailed())
            {
                //  最终失败，执行失败方法
                await Failed_Internal(context, data);
            }

            await ProcessEnd_Internal(res, data, context);
            return res;
        }

        
        /// <summary>
        ///   具体递归执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<ResultMo> Recurs(TaskContext context, TaskReqData data)
        {
            ResultMo res;

            var directExcuteTimes = 0;
            do
            {
                //  直接执行
                res = await Do_Internal(context, data);
                if (res == null)
                    throw new ArgumentNullException($"{this.GetType().Name} return null！");

                // 判断是否失败回退
                if (res.IsTaskFailed())
                    await Revert_Internal(context, data);
                
                directExcuteTimes++;
                context.exced_times++;
            }

            // 判断是否执行直接重试 
            while (res.IsTaskFailed() && directExcuteTimes < context.task_meta.continue_times);

            return res;
        }

        #endregion

        #region 实现，重试，失败, 结束基础内部扩展方法

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns> sys_ret = (int)SysResultTypes.RunFailed 系统会字段判断是否满足重试条件执行重试    </returns>
        internal abstract Task<ResultMo> Do_Internal(TaskContext context, TaskReqData data);

        /// <summary>
        ///  执行失败回退操作
        ///   如果设置了重试配置，调用后重试
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        internal abstract Task Revert_Internal(TaskContext context, TaskReqData data);

        /// <summary>
        ///  最终执行失败会执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        internal abstract Task Failed_Internal(TaskContext context, TaskReqData data);

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
        internal virtual Task ProcessEnd_Internal(ResultMo taskRes, TaskReqData data, TaskContext context)
        {
            return Task.CompletedTask;
        }
        
        #endregion
    }
}