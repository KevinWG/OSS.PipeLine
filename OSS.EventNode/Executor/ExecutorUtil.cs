using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.Common.Resp;
using OSS.EventNode.Mos;
using OSS.EventTask;
using OSS.EventTask.Extention;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode.Executor
{
    internal class ExecutorUtil
    {
        // 根据任务结果格式化当前节点结果， 外部循环使用
        internal static bool FormatNodeErrorResp<TTRes>(NodeResp<TTRes> nodeResp, TaskResp<Resp> taskResp,
            TaskMeta tMeta)
            where TTRes : Resp, new()
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
                    case NodeResultAction.RevrtAllOnFailed:
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
        internal static async Task<TaskResp<Resp>> TryGetTaskItemResult<TTData>(TTData data, IEventTask<TTData> task,
            int triedTimes)
            where TTData : class
        {
            //if (nodeInsType == InstanceType.Stand && task.InstanceTaskType == InstanceType.Domain)
            //{
            //    return new TaskResponse<Resp>().WithError(TaskRunStatus.RunFailed, new RunCondition(),
            //        "Stand Node can't use Domain Task!");
            //}
            try
            {
                return await task.Run(data, triedTimes);
            }
            catch (Exception ex)
            {
                LogUtil.Error($"An error occurred while the task was running. Detail：{ex}", task.TaskMeta.node_id, EventTaskProvider.ModuleName);
            }

            return new TaskResp<Resp>().WithError(TaskRunStatus.RunFailed, new RunCondition(),
                "Task of node run error!");
        }

        //  任务结果转化到节点结果
        internal static TTRes ConvertToNodeResult<TTRes>(Resp taskRes)
            where TTRes : Resp, new()
        {
            if (taskRes is TTRes nres)
            {
                return nres;
            }

            return new TTRes().WithResp(taskRes);// taskRes.ConvertToResultInherit<TTRes>();
        }



        //  尝试回退任务
        internal static Task<bool> TryRevertTask<TTData>(IEventTask<TTData> task, TTData data,  int triedTimes)
        {
            try
            {
                return task.Revert(data,triedTimes);
            }
            catch (Exception e)
            {
                LogUtil.Error($"Task revert error！ detail:{e}", task.TaskMeta.task_id, EventTaskProvider.ModuleName);
            }

            return Task.FromResult(false);
        }

    }
}
