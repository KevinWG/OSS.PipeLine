using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
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
        internal async Task<ResultMo> Process(TaskBaseContext context)
        {
            var res = await Recurs(context);

            // 判断是否间隔执行,生成重试信息
            if (res.IsTaskFailed() && context.interval_times < RetryConfig?.interval_times)
            {
                context.interval_times++;
                await SaveTaskContext(context);
                res.sys_ret = (int) TaskResultType.WatingRetry;
            }

            if (res.IsTaskFailed())
            {
                //  最终失败，执行失败方法
                await Failed_Internal(context);
            }

            return res;
        }

        /// <summary>
        ///   具体递归执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<ResultMo> Recurs(TaskBaseContext context)
        {
            ResultMo res;

            var directExcuteTimes = 0;
            do
            {
                //  直接执行
                res = await Do_Internal(context);
                if (res == null)
                    throw new ArgumentNullException($"{this.GetType().Name} return null！");

                // 判断是否失败回退
                if (res.IsTaskFailed())
                    await Revert_Internal(context);
                
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
        internal abstract Task<ResultMo> Do_Internal(TaskBaseContext context);

        /// <summary>
        ///  执行失败回退操作
        ///   如果设置了重试配置，调用后重试
        /// </summary>
        /// <param name="context"></param>
        internal abstract Task Revert_Internal(TaskBaseContext context);

        /// <summary>
        ///  最终执行失败会执行
        /// </summary>
        /// <param name="context"></param>
        internal abstract Task Failed_Internal(TaskBaseContext context);
        
        #endregion

    }
    
    public abstract partial class BaseTask<TPara, TRes> : BaseTask
        where TRes : ResultMo, new()
    {
        #region 具体任务执行入口

        /// <summary>
        ///   任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns>  </returns>
        public async Task<TRes> Process(TaskContext<TPara> context)
        {
            return (await base.Process(context)) as TRes;
        }
        
        #endregion

        #region 实现，重试，失败 执行  重写父类方法

        internal override async Task<ResultMo> Do_Internal(TaskBaseContext context)
        {
            return await Do((TaskContext<TPara>)context);
        }

        internal override Task Failed_Internal(TaskBaseContext context)
        {
            return Failed((TaskContext<TPara>)context);
        }

        internal override Task Revert_Internal(TaskBaseContext context)
        {
            return Revert((TaskContext<TPara>)context);
        }

        internal override Task SaveTaskContext(TaskBaseContext context)
        {
            return _contextKepper.Invoke((TaskContext<TPara>)context);
        }

        #endregion


        #region 实现，重试，失败 执行  扩展方法

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns>  特殊：ret=-100（EventFlowResult.Failed）任务处理失败，执行回退，并根据重试设置发起重试</returns>
        protected abstract Task<TRes> Do(TaskContext<TPara> context);

        /// <summary>
        ///  执行失败回退操作
        ///   如果设置了重试配置，调用后重试
        /// </summary>
        /// <param name="context"></param>
        protected internal virtual Task Revert(TaskContext<TPara> context)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///  最终执行失败会执行
        /// </summary>
        /// <param name="context"></param>
        protected virtual Task Failed(TaskContext<TPara> context)
        {
            return Task.CompletedTask;
        }

        #endregion

       
    }
}