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
    public abstract partial class BaseTask<TTReq,TTRes> : BaseMetaTask<TTReq, TTRes>
        where TTReq : BaseReq
        where TTRes : ResultMo, new()
    {
        #region 任务进入入口
        
        // 串联流程，以及框架内部异常处理
        internal async Task<TTRes> Run(TTContext context)
        {
            
            string errorMsg;
            //var res = default(TTRes);
            context.run_status = TaskRunStatus.WaitToRun;
            try
            {
                var checkRes = RunCheck(context);
                if (!checkRes.IsSuccess())
                    return checkRes;

                // 【1】 执行起始方法
                await RunStart(context);

                // 【2】  执行核心方法
                context.resp = await Runing(context);

                // 【3】 执行结束方法
                await RunEnd(context.resp, context);
                return context.resp;
            }
            catch (ResultException e)
            {
                errorMsg = e.ToString();
                if (context.resp == null)
                    context.resp = e.ConvertToReultInherit<TTRes>(); //.ConvertToReult<TTRes>();
            }
            catch (Exception e)
            {
                errorMsg = e.ToString();
                if (context.resp == null)
                    context.resp = new TTRes().WithResult(SysResultTypes.ApplicationError, "Error occurred during task [Run]!");
            }

            LogUtil.Error(
                $"sys_ret:{ context.resp.sys_ret}, ret:{ context.resp.ret},msg:{ context.resp.msg}, Detail:{errorMsg}"
                , TaskMeta.task_key, ModuleName);

            await TrySaveTaskContext(context);
            return context.resp;
        }


        #endregion

        #region 生命周期扩展方法

        /// <summary>
        /// 任务开始方法
        /// </summary>
        /// <param name="context">请求的上下文</param>
        /// <returns></returns>
        protected virtual Task RunStart(TTContext context)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 任务结束方法
        /// </summary>
        /// <param name="taskRes"></param>
        /// <param name="context">请求的上下文</param>
        /// <returns></returns>
        protected virtual Task RunEnd(TTRes taskRes, TTContext context)
        {
            return Task.CompletedTask;
        }

        internal virtual TTRes RunCheck(TTContext context)
        {
            if (string.IsNullOrEmpty(TaskMeta?.task_key))
                return new TTRes().WithResult(SysResultTypes.ApplicationError, "Task metainfo is null!");
               
            if (context.task_condition == null)
                return new TTRes().WithResult(SysResultTypes.ApplicationError, "Task run condition data can't be null！");
         
            return new TTRes();
        }


        #endregion

        #region 扩展方法（实现，回退，失败）  扩展方法

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="runStatus"></param>
        /// <returns> 
        ///  runStatus = TaskRunStatus.RunFailed 系统会字段判断是否满足重试条件执行重试
        /// </returns>
        protected abstract Task<TTRes> Do(TTContext context,out TaskRunStatus runStatus);

        /// <summary>
        ///  执行失败回退操作
        ///   如果设置了重试配置，调用后重试
        /// </summary>
        /// <param name="context"></param>
        protected internal virtual Task Revert(TTContext context)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///  最终失败执行方法
        /// </summary>
        /// <param name="context"></param>
        protected virtual Task Failed(TTContext context)
        {
            return Task.CompletedTask;
        }


        #endregion

        #region 辅助方法

        /// <summary> 
        ///   任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns>  </returns>
        private async Task<TTRes> Runing(TTContext context)
        {
            var runCondition = context.task_condition;
            context.run_status = await Recurs(context);
            // 判断是否间隔执行,生成重试信息
            if (context.run_status.IsFailed() && runCondition.interval_times < TaskMeta.interval_times)
            {
                runCondition.interval_times++;
                await TrySaveTaskContext(context);
                context.run_status = TaskRunStatus.RunPaused; // TaskResultType.WatingActivation;
            }

            if (context.run_status.IsFailed())
            {
                //  最终失败，执行失败方法
                await Failed(context);
            }

            return context.resp ?? (context.resp = new TTRes()
            {
                sys_ret = (int) SysResultTypes.NoResponse,
                ret = (int) ResultTypes.InnerError,
                msg = "Have no response during task [Do]!"
            });
        }

        /// <summary>
        ///   具体递归执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<TTRes> Recurs(TTContext context)
        {
         
            var directProcessTimes = 0;
            do
            {
                //  直接执行
                context.resp = await TryDo(context);
          
                // 判断是否失败回退
                if (context.run_status.IsFailed())
                    await Revert(context);

                directProcessTimes++;
                context.task_condition.exced_times++;
            }
            // 判断是否执行直接重试 
            while (context.run_status.IsFailed() && directProcessTimes < TaskMeta.continue_times);

            return context.resp;
        }

        //  保证外部异常不会对框架内部运转造成影响
        //  如果失败返回 RunFailed 保证系统后续重试处理
        private async Task<TTRes> TryDo(TTContext context)
        {
            try
            {
                context.resp=await Do(context, out var runStatus);
                context.run_status = runStatus;
                return context.resp;
            }
            catch (Exception e)
            {
                context.run_status = TaskRunStatus.RunFailed;
                context.resp =
                    new TTRes().WithResult(SysResultTypes.ApplicationError, "Error occurred during task [Do]!");

                LogUtil.Error(
                    $"sys_ret:{context.resp.sys_ret}, ret:{context.resp.ret},msg:{context.resp.msg}, Detail:{e}"
                    , TaskMeta.task_key, ModuleName);

                return context.resp;
            }
        }
        
        #endregion
    }
}