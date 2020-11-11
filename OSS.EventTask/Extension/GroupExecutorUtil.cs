using System.Threading.Tasks;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask.Extension
{
    internal class GroupExecutorUtil
    {
        // 根据任务结果格式化当前节点结果， 外部循环使用
        // 群组内的任务结果只能是成功或失败
        internal static GroupExecuteStatus FormatNodeErrorResp<TTRes>(TaskResp<TTRes> taskResp, TaskMeta tMeta)
            where TTRes : class, new()
        {
            if (tMeta.failed_effect != FailedEffect.FailedAll || taskResp.run_status.IsCompleted())
            {
                return GroupExecuteStatus.Complete;
            }

            if (tMeta.revert_effect== RevertEffect.RevertAll)
            {
                return GroupExecuteStatus.Failed | GroupExecuteStatus.Revert;
            }

            return GroupExecuteStatus.Failed;
        }

        //  尝试获取任务执行结果
        internal static Task<TaskResp<TTRes>> TryGetTaskItemResult<TTData,TTRes>(TTData data, IEventTask<TTData, TTRes> task)
            where TTData : class
            where TTRes : class, new()
        {
            //try
            //{
                return  task.Process(data);
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
