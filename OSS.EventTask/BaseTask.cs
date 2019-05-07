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
    public abstract partial class BaseTask<TTReq, TTRes, TReq>

    {
        #region 任务进入入口

        // 串联流程，以及框架内部异常处理
        public async Task<TaskResponse<TTRes>> Run(TTReq req, RunCondition runCondition)
        {
            var taskResp = new TaskResponse<TTRes>
            {
                run_status = TaskRunStatus.WaitToRun,
                task_condition = runCondition
            };

            await TryRun(req, taskResp);
            return taskResp;
        }

        private async Task TryRun(TTReq req, TaskResponse<TTRes> taskResp)
        {
            string errorMsg;
            try
            {
                var checkRes = RunCheck(req, taskResp.task_condition);
                if (!checkRes.IsSuccess())
                {
                    taskResp.resp = checkRes;
                    return ;
                }

                // 【1】 执行起始方法
                await RunStart(req, taskResp.task_condition);

                // 【2】  执行核心方法
                await Runing(req, taskResp);

                // 【3】 执行结束方法
                await RunEnd(req, taskResp);
                return ;
            }
            catch (ResultException e)
            {
                errorMsg = e.ToString();
                if (taskResp.resp == null)
                    taskResp.resp = e.ConvertToReultInherit<TTRes>(); //.ConvertToReult<TTRes>();
            }
            catch (Exception e)
            {
                errorMsg = e.ToString();
                if (taskResp.resp == null)
                    taskResp.resp = new TTRes().WithResult(SysResultTypes.ApplicationError,
                        "Error occurred during task [Run]!");
            }

            var resp = taskResp.resp;
            LogUtil.Error($"sys_ret:{resp.sys_ret}, ret:{resp.ret},msg:{resp.msg}, Detail:{errorMsg}", TaskMeta.task_key,
                ModuleName);
            await TrySaveTaskContext(req, taskResp);
        }

        #endregion

        #region 生命周期扩展方法

        /// <summary>
        /// 任务开始方法
        /// </summary>
        /// <param name="req"></param>
        /// <param name="runCondition"></param>
        /// <returns></returns>
        protected virtual Task RunStart(TTReq req, RunCondition runCondition)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 任务结束方法
        /// </summary>
        /// <param name="req"></param>
        /// <param name="context">请求的上下文</param>
        /// <returns></returns>
        protected virtual Task RunEnd(TTReq req,TaskResponse<TTRes> context)
        {
            return Task.CompletedTask;
        }

        internal virtual TTRes RunCheck(TTReq context, RunCondition runCondition)
        {
            if (string.IsNullOrEmpty(TaskMeta?.task_key))
                return new TTRes().WithResult(SysResultTypes.ApplicationError, "Task metainfo is null!");

            if (runCondition == null)
                return new TTRes().WithResult(SysResultTypes.ApplicationError,
                    "Task run condition data can't be null！");

            return new TTRes();
        }


        #endregion

        #region 扩展方法（实现，回退，失败）  扩展方法

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="req"></param>
        /// <param name="runStatus"></param>
        /// <returns> 
        ///  runStatus = TaskRunStatus.RunFailed 系统会字段判断是否满足重试条件执行重试
        /// </returns>
        protected abstract Task<TTRes> Do(TTReq req, out TaskRunStatus runStatus);

        /// <summary>
        ///  执行失败回退操作
        ///   如果设置了重试配置，调用后重试
        /// </summary>
        /// <param name="req"></param>
        protected internal virtual Task Revert(TTReq req)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///  最终失败执行方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="taskResp"></param>
        protected virtual Task Failed(TTReq context,TaskResponse<TTRes> taskResp)
        {
            return Task.CompletedTask;
        }


        #endregion

        #region 辅助方法

        /// <summary> 
        ///   任务的具体执行
        /// </summary>
        /// <param name="req"></param>
        /// <param name="taskResp"></param>
        /// <returns>  </returns>
        private async Task Runing(TTReq req, TaskResponse<TTRes> taskResp)
        {
            await Recurs(req, taskResp);
            
            // 判断是否间隔执行,生成重试信息
            var runCondition = taskResp.task_condition;

            if (taskResp.run_status.IsFailed() && runCondition.interval_times < TaskMeta.interval_times)
            {
                runCondition.interval_times++;
                taskResp.run_status = TaskRunStatus.RunPaused; // TaskResultType.WatingActivation;

                await TrySaveTaskContext(req, taskResp);
            }

            if (taskResp.run_status.IsFailed())
            {
                //  最终失败，执行失败方法
                await Failed(req,taskResp);
            }
        }

        /// <summary>
        ///   具体递归执行
        /// </summary>
        /// <param name="req"></param>
        /// <param name="taskResp"></param>
        /// <returns></returns>
        private async Task Recurs(TTReq req, TaskResponse<TTRes> taskResp)
        {
            var directProcessTimes = 0;
            do
            {
                //  直接执行
                await TryDo(req, taskResp);

                // 判断是否失败回退
                if (taskResp.run_status.IsFailed())
                    await Revert(req);

                directProcessTimes++;
                taskResp.task_condition.exced_times++;
            }
            // 判断是否执行直接重试 
            while (taskResp.run_status.IsFailed() && directProcessTimes < TaskMeta.continue_times);

    
        }

        //  保证外部异常不会对框架内部运转造成影响
        //  如果失败返回 RunFailed 保证系统后续重试处理
        private async Task TryDo(TTReq req, TaskResponse<TTRes> taskResp)
        {
            try
            {
                taskResp.resp = await Do(req, out var runStatus)
                    ?? new TTRes().WithResult(SysResultTypes.NoResponse, "Have no response during task [Do]!"); 

                taskResp.run_status = runStatus;
            }
            catch (Exception e)
            {
                taskResp.run_status = TaskRunStatus.RunFailed;
                taskResp.resp =
                    new TTRes().WithResult(SysResultTypes.ApplicationError, "Error occurred during task [Do]!");

                LogUtil.Error(
                    $"sys_ret:{taskResp.resp.sys_ret}, ret:{taskResp.resp.ret},msg:{taskResp.resp.msg}, Detail:{e}"
                    , TaskMeta.task_key, ModuleName);

            }
        }

        #endregion
    }
}