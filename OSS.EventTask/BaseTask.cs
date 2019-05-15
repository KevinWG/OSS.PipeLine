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
    public abstract partial class BaseTask<TTReq, TTRes>
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

        async Task<TaskResponse<ResultMo>> IBaseTask<TTReq>.Run(TTReq req, RunCondition runCondition)
        {
            var taskResp = await Run(req, runCondition);
            return new TaskResponse<ResultMo>()
            {
                run_status = taskResp.run_status,
                resp = taskResp.resp,
                task_condition = taskResp.task_condition
            };
        }

        // 运行
        private async Task TryRun(TTReq req, TaskResponse<TTRes> taskResp)
        {
            string errorMsg;
            try
            {
                // 【1】 执行起始方法 附加校验
                var checkRes = await RunCheck(req, taskResp);
                if (!checkRes)
                    return;

                // 【2】  执行核心方法
                await Runing(req, taskResp);

                // 【3】 执行结束方法
                await RunEnd(req, taskResp);

                //  结束 如果是中断处理，保存请求相关信息
                if (taskResp.run_status==TaskRunStatus.RunPaused)
                    await TrySaveTaskContext(req, taskResp);
                
                return;
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
                    taskResp.resp = new TTRes().WithResult(SysResultTypes.ApplicationError,"Error occurred during task [Run]!");
            }

            taskResp.run_status = TaskRunStatus.RunFailed;
            var resp = taskResp.resp;
            LogUtil.Error($"sys_ret:{resp.sys_ret}, ret:{resp.ret},msg:{resp.msg}, Detail:{errorMsg}",
                TaskMeta.task_id,ModuleName);
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
        protected virtual Task<ResultMo> RunStartCheck(TTReq req, RunCondition runCondition)
        {
            return Task.FromResult(new ResultMo());
        }

        /// <summary>
        /// 任务结束方法
        /// </summary>
        /// <param name="req"></param>
        /// <param name="context">请求的上下文</param>
        /// <returns></returns>
        protected virtual Task RunEnd(TTReq req, TaskResponse<TTRes> context)
        {
            return Task.CompletedTask;
        }


        private async Task<bool> RunCheck(TTReq req, TaskResponse<TTRes> taskResp)
        {
            var checkInRes = RunCheckInternal(req, taskResp.task_condition);
            if (!checkInRes.IsSuccess())
            {
                taskResp.run_status = TaskRunStatus.RunFailed;
                taskResp.resp = checkInRes;
                return false;
            }

            var res = await RunStartCheck(req, taskResp.task_condition);
            if (!res.IsSuccess())
            {
                taskResp.run_status = TaskRunStatus.RunFailed;
                taskResp.resp = res.ConvertToResultInherit<TTRes>();
                return false;
            }

            return true;
        }

        internal virtual TTRes RunCheckInternal(TTReq context, RunCondition runCondition)
        {
            if (string.IsNullOrEmpty(TaskMeta?.task_id))
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
        /// <returns> 
        ///  runStatus = TaskRunStatus.RunFailed 系统会字段判断是否满足重试条件执行重试
        /// </returns>
        protected abstract Task<DoResponse<TTRes>> Do(TTReq req);

        /// <summary>
        ///  执行失败回退操作
        ///   如果设置了重试配置，调用后重试
        /// </summary>
        /// <param name="req"></param>
        public virtual Task<bool> Revert(TTReq req)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        ///  最终失败执行方法
        /// </summary>
        /// <param name="req"></param>
        /// <param name="taskResp"></param>
        protected virtual Task Failed(TTReq req, TaskResponse<TTRes> taskResp)
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
            }

            if (taskResp.run_status.IsFailed())
            {
                //  最终失败，执行失败方法
                await Failed(req, taskResp);
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
                var doRes = await Do(req);

                taskResp.run_status = doRes.run_status;
                taskResp.resp = doRes.resp
                                ?? new TTRes().WithResult(SysResultTypes.NoResponse,
                                    "Have no response during task [Do]!");
            }
            catch (Exception e)
            {
                taskResp.run_status = TaskRunStatus.RunFailed;
                taskResp.resp =
                    new TTRes().WithResult(SysResultTypes.ApplicationError, "Error occurred during task [Do]!");

                LogUtil.Error(
                    $"sys_ret:{taskResp.resp.sys_ret}, ret:{taskResp.resp.ret},msg:{taskResp.resp.msg}, Detail:{e}"
                    , TaskMeta.task_id, ModuleName);

            }
        }

        #endregion
    }
}