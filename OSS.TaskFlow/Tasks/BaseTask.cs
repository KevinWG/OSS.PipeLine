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
        /// <returns>  </returns>
        internal async Task<ResultMo> Process(TaskContext context, TaskReqData data)
        {
            var checkRes = InitailTask(context);
            if (!checkRes.IsSysResultType(SysResultTypes.None))
                return checkRes;

            var res = await Recurs(context, data);

            // 判断是否间隔执行,生成重试信息
            if (res.IsTaskFailed() && context.interval_times < RetryConfig?.interval_times)
            {
                context.interval_times++;
                await SaveTaskContext(context, data);
                res.sys_ret = (int) TaskResultType.WatingActivation;
            }

            if (res.IsTaskFailed())
            {
                //  最终失败，执行失败方法
                await Failed_Internal(context, data);
            }

            return res;
        }

        /// <summary>
        ///   具体递归执行
        /// </summary>
        /// <param name="context"></param>
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
            while (res.IsTaskFailed() && directExcuteTimes < RetryConfig?.continue_times);

            return res;
        }
        #endregion

        #region 实现，重试，失败 执行  基础方法

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns>  特殊：sys_ret= SysResultType.TaskFailed 任务处理失败，执行回退，并根据重试设置发起重试</returns>
        internal abstract Task<ResultMo> Do_Internal(TaskContext context, TaskReqData data);

        /// <summary>
        ///  执行失败回退操作
        ///   如果设置了重试配置，调用后重试
        /// </summary>
        /// <param name="context"></param>
        internal abstract Task Revert_Internal(TaskContext context, TaskReqData data);

        /// <summary>
        ///  最终执行失败会执行
        /// </summary>
        /// <param name="context"></param>
        internal abstract Task Failed_Internal(TaskContext context, TaskReqData data);

        #endregion

        /// <summary>
        ///  运行校验
        /// </summary>
        /// <returns></returns>
        internal virtual ResultMo InitailTask(TaskContext context)
        {
            if (context.task_meta == null)
            {
                return new ResultMo((int)TaskResultType.ConfigError, (int)ResultTypes.InnerError, $"can't find metainfo in task with type '{this.GetType().Name}'");
            }
            if (string.IsNullOrEmpty(context.task_meta.task_key))
            {
                return new ResultMo((int)TaskResultType.ConfigError, (int)ResultTypes.InnerError, $"task key can't be null!");
            }
            return new ResultMo();
        }
    }
}