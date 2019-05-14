using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.EventNode.MetaMos;
using OSS.EventNode.Mos;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;

namespace OSS.EventNode.Executor
{
    internal class ExecutorUtil
    {
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
                        nodeResp.resp = ConvertToNodeResp<TTRes>(taskResp.resp);
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

        internal static async Task<TaskResponse<ResultMo>> TryGetTaskItemResult<TTReq>(TTReq req, IBaseTask<TTReq> task,
            RunCondition taskRunCondition, InstanceType nodeInsType)
            where TTReq : ExcuteReq
        {
            if (nodeInsType == InstanceType.Stand && task.InstanceTaskType == InstanceType.Domain)
            {
                return new TaskResponse<ResultMo>().WithError(TaskRunStatus.RunFailed, new RunCondition(),
                    "Stand Node can't use Domain Task!");
            }

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


        internal static TTRes ConvertToNodeResp<TTRes>(ResultMo taskResp)
            where TTRes : ResultMo, new()
        {
            if (taskResp is TTRes nres)
            {
                return nres;
            }

            return taskResp.ConvertToResultInherit<TTRes>();
        }

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
