using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.Common.Plugs.LogPlug;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;

namespace OSS.EventTask
{
    public abstract partial class BaseTask
    {
        #region 任务进入入口

        /// <summary> 
        ///   任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns>  </returns>
        internal virtual async Task<ResultMo> Process_Internal(TaskContext context, TaskReqData data)
        {
            ResultMo res;
            try
            {
                res = await Recurs(context, data);

                // 判断是否间隔执行,生成重试信息
                if (res.IsRunFailed() && context.interval_times < context.task_meta.interval_times)
                {
                    context.interval_times++;
                    await SaveTaskContext(context, data);
                    res.sys_ret = (int) SysResultTypes.RunPause; // TaskResultType.WatingActivation;
                }

                if (res.IsRunFailed())
                {
                    //  最终失败，执行失败方法
                    await Failed_Internal(context, data);
                }
            }
            catch (ResultException e)
            {
                res = e.ConvertToReult();
                LogUtil.Error($"Error occurred during task execution! sys_ret:{res.sys_ret}, ret:{res.ret},msg:{res.msg}"
                    , "TaskFlow_TaskProcess", "Oss.TaskFlow");
                await SaveTaskContext(context, data);
            }
            return res;
        }

        #endregion

        #region 基础内部扩展方法（实现，回退，失败）

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

        #endregion
        
        #region 辅助方法


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
                if (res.IsRunFailed())
                    await Revert_Internal(context, data);

                directExcuteTimes++;
                context.exced_times++;
            }
            // 判断是否执行直接重试 
            while (res.IsRunFailed() && directExcuteTimes < context.task_meta.continue_times);

            return res;
        }


        // 状态有效判断
        internal static ResultMo CheckTaskContext(TaskContext context)
        {
            //  todo  状态有效判断等
            if (!string.IsNullOrEmpty(context.task_meta?.task_key))
                return new ResultMo();

            throw new ResultException(SysResultTypes.ConfigError, ResultTypes.InnerError, "task metainfo has error!");
        }
        
        #endregion
    }
}