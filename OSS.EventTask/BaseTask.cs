using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.Common.Plugs.LogPlug;
using OSS.EventTask.Interfaces;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;

namespace OSS.EventTask
{
    public abstract partial class BaseTask<TTContext, TTRes>:IBaseTask
        where TTContext : TaskContext
        where TTRes : ResultMo, new()
    {
        #region 任务进入入口

        /// <summary>
        ///  执行任务
        /// </summary>
        /// <param name="context">任务的上下文数据信息</param>
        /// <returns></returns>
        public  Task<TTRes> Process(TTContext context)
        {
            return Process(context, new RunCondition());
        }

        /// <summary>
        ///  执行任务
        /// </summary>
        /// <param name="context">任务的上下文数据信息</param>
        /// <param name="runCondition">运行情况信息(重试校验依据)</param>
        /// <returns></returns>
        public Task<TTRes> ProcessWithRetry(TTContext context, RunCondition runCondition)
        {
            return Process(context, runCondition);
        }

      

        /// <summary> 
        ///   任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="runCondition"></param>
        /// <returns>  </returns>
        private async Task<TTRes> Processing(TTContext context, RunCondition runCondition)
        {
            var res = await Recurs(context, runCondition);
            // 判断是否间隔执行,生成重试信息
            if (res.IsRunFailed() && runCondition.interval_times < context.task_meta.interval_times)
            {
                runCondition.interval_times++;
                await TrySaveTaskContext(context,runCondition);
                res.sys_ret = (int) SysResultTypes.RunPause; // TaskResultType.WatingActivation;
            }

            if (res.IsRunFailed())
            {
                //  最终失败，执行失败方法
                await Failed(context);
            }
            return res;
        }

        #endregion
        
        #region 生命周期扩展方法

    

        /// <summary>
        /// 任务开始方法
        /// </summary>
        /// <param name="context">请求的上下文</param>
        /// <returns></returns>
        protected virtual Task ProcessStart(TTContext context)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 任务结束方法
        /// </summary>
        /// <param name="taskRes">任务结果 :
        ///  sys_ret = (int)SysResultTypes.RunFailed 表明最终执行失败，
        ///  sys_ret = (int)SysResultTypes.RunPause 表示符合间隔重试条件，会通过 contextKeeper 保存信息后续唤起
        /// </param>
        /// <param name="context">请求的上下文</param>
        /// <returns></returns>
        protected virtual Task ProcessEnd(TTRes taskRes, TTContext context)
        {
            return Task.CompletedTask;
        }

        internal virtual ResultMo ProcessCheck(TTContext context,RunCondition runCondition)
        {
            //  todo  状态有效判断等
            if (string.IsNullOrEmpty(context.task_meta?.task_key))
                return new ResultMo(SysResultTypes.ConfigError, ResultTypes.InnerError, "task metainfo has error!");

            if (runCondition==null)
                return new ResultMo(SysResultTypes.ConfigError, ResultTypes.InnerError, "运行数据不能为空！");

            return new ResultMo();

        }
        #endregion

        #region 扩展方法（实现，回退，失败）  扩展方法

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns> sys_ret = (int)SysResultTypes.RunFailed 系统会字段判断是否满足重试条件执行重试    </returns>
        protected abstract Task<TTRes> Do(TTContext context);

        /// <summary>
        ///  执行失败回退操作
        ///   如果设置了重试配置，调用后重试
        /// </summary>
        /// <param name="context"></param>
        protected internal virtual Task Revert(TaskContext context)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///  最终失败执行方法
        /// </summary>
        /// <param name="context"></param>
        protected virtual Task Failed(TaskContext context)
        {
            return Task.CompletedTask;
        }


        #endregion

        #region 辅助方法
        private async Task<TTRes> Process(TTContext context, RunCondition runCondition)
        {
            TTRes res;
            try
            {
                var checkRes = ProcessCheck(context, runCondition);
                if (!checkRes.IsSuccess())
                    return checkRes.ConvertToResultInherit<TTRes>();

                // 【1】 执行起始方法
                await ProcessStart(context);

                // 【2】  执行核心方法
                res = await Processing(context, runCondition);

                // 【3】 执行结束方法
                await ProcessEnd(res, context);
                return res;
            }
            catch (ResultException e)
            {
                res = e.ConvertToReult().ConvertToResultInherit<TTRes>();
                LogUtil.Error(
                    $"Error occurred during task execution! sys_ret:{res.sys_ret}, ret:{res.ret},msg:{res.msg}"
                    , "TaskFlow_TaskProcess", "Oss.TaskFlow");
                await TrySaveTaskContext(context, runCondition);
            }
            return res;
        }


        /// <summary>
        ///   具体递归执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<TTRes> Recurs(TTContext context, RunCondition runCondition)
        {
            TTRes res;
            var directExcuteTimes = 0;
            do
            {
                //  直接执行
                res = await Do(context);
                if (res == null)
                    throw new ArgumentNullException($"{this.GetType().Name} return null！");

                // 判断是否失败回退
                if (res.IsRunFailed())
                    await Revert(context);

                directExcuteTimes++;
                runCondition.exced_times++;
            }
            // 判断是否执行直接重试 
            while (res.IsRunFailed() && directExcuteTimes < context.task_meta.continue_times);

            return res;
        }


        // 状态有效判断

        
        #endregion
    }
}