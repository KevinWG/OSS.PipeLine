using System.Threading.Tasks;
using OSS.EventNode.Mos;
using OSS.EventTask.Extention;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode.Executor
{
    internal class ExecutorUtil
    {
        // 根据任务结果格式化当前节点结果， 外部循环使用
        internal static bool FormatNodeErrorResp<TTRes>(GroupTaskResp<TTRes> nodeResp, TaskResp<TTRes> taskResp,
            TaskMeta tMeta)
            where TTRes : class, new()
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
                    case NodeResultAction.RevertAllOnFailed:
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
        internal static Task<TaskResp<TTRes>> TryGetTaskItemResult<TTData,TTRes>(TTData data, IEventTask<TTData, TTRes> task,
            int triedTimes)
            where TTData : class
            where TTRes : class, new()
        {
            //try
            //{
                return  task.Run(data, triedTimes);
            //}
            //catch (Exception ex)
            //{
            //    LogUtil.Error($"An error occurred while the task was running. Detail：{ex}", task.TaskMeta.node_id, EventTaskProvider.ModuleName);
            //}

            //return new TaskResp<Resp>().WithError(TaskRunStatus.RunFailed, new RunCondition(),
            //    "Task of node run error!");
        }

        //  任务结果转化到节点结果
        internal static TTRes ConvertToNodeResult<TTRes>(TTRes taskRes)
            where TTRes : class, new()
        {
            if (taskRes is TTRes nres)
            {
                return nres;
            }
            return null;
            //return new TTRes().WithResp(taskRes);// taskRes.ConvertToResultInherit<TTRes>();
        }



        //  尝试回退任务
        internal static Task<bool> TryRevertTask<TTData>(IEventTask<TTData> task, TTData data)
        {
            //try
            //{
                return task.Revert(data);
            //}
            //catch (Exception e)
            //{
            //    LogUtil.Error($"Task revert error！ detail:{e}", task.TaskMeta.task_id, EventTaskProvider.ModuleName);
            //}

            //return Task.FromResult(false);
        }

    }
}
