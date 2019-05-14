using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.EventNode.Mos;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;

namespace OSS.EventNode.Executor
{
    internal class ExecutorUtil
    {
        // 根据任务结果格式化当前节点结果， 外部循环使用
        internal static bool FormatNodeErrorResp<TTRes>(NodeResponse<TTRes> nodeResp, TaskResponse<ResultMo> taskResp,
            TaskMeta tMeta)
            where TTRes : ResultMo, new()
        {
            var status = NodeStatus.ProcessCompoleted;
            if (!taskResp.run_status.IsCompleted())
            {
                var haveError = true;
                switch (tMeta.node_action)
                {
                    case NodeResultAction.PauseOnFailed:
                        status = NodeStatus.ProcessPaused;
                        break;
                    case NodeResultAction.FailedOnFailed:
                        status = taskResp.run_status == TaskRunStatus.RunFailed
                            ? NodeStatus.ProcessFailed
                            : NodeStatus.ProcessPaused;
                        break;
                    case NodeResultAction.FailedRevrtOnFailed:
                        status = taskResp.run_status == TaskRunStatus.RunFailed
                            ? NodeStatus.ProcessFailedRevert
                            : NodeStatus.ProcessPaused;
                        break;
                    default:
                        haveError = false;
                        break;
                }

                if (haveError)
                {
                    if (status < nodeResp.node_status)
                    {
                        nodeResp.node_status = status;
                        nodeResp.resp = ConvertToNodeResult<TTRes>(taskResp.resp);
                    }

                    return true;
                }
            }

            if (nodeResp.node_status == NodeStatus.ProcessCompoleted && taskResp.resp is TTRes nres)
            {
                nodeResp.resp = nres;
            }

            return false;
        }
        //  尝试获取任务执行结果
        internal static async Task<TaskResponse<ResultMo>> TryGetTaskItemResult<TTReq>(TTReq req, IBaseTask<TTReq> task,
            RunCondition taskRunCondition)
            where TTReq : class
        {
            //if (nodeInsType == InstanceType.Stand && task.InstanceTaskType == InstanceType.Domain)
            //{
            //    return new TaskResponse<ResultMo>().WithError(TaskRunStatus.RunFailed, new RunCondition(),
            //        "Stand Node can't use Domain Task!");
            //}
            try
            {
                return await task.Run(req, taskRunCondition);
            }
            catch (Exception ex)
            {
                LogUtil.Error($"An error occurred while the task was running. Detail：{ex}", task.TaskMeta.node_id, task.ModuleName);
            }

            return new TaskResponse<ResultMo>().WithError(TaskRunStatus.RunFailed, new RunCondition(),
                "Task of node run error!");
        }

        //  任务结果转化到节点结果
        internal static TTRes ConvertToNodeResult<TTRes>(ResultMo taskRes)
            where TTRes : ResultMo, new()
        {
            if (taskRes is TTRes nres)
            {
                return nres;
            }

            return taskRes.ConvertToResultInherit<TTRes>();
        }



        //  尝试回退任务
        internal static Task<bool> TryRevertTask<TTReq>(IBaseTask<TTReq> task, TTReq req)
        {
            try
            {
                return task.Revert(req);
            }
            catch (Exception e)
            {
                LogUtil.Error($"Task revert error！ detail:{e}", task.TaskMeta.task_id, task.ModuleName);
            }

            return Task.FromResult(false);
        }

    }
}
