using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask.Extension
{
    public static class ParallelGroupExtension
    {
        //   并行执行任务扩展
        internal static async Task<GroupExecuteStatus> Executing_Parallel<TTData, TTRes>(
            this EventTask.GroupEventTask<TTData, TTRes> groupEvent, TTData data,
            GroupEventTaskResp<TTRes> nodeResp, IList<IEventTask<TTData, TTRes>> tasks)
            where TTData : class
            where TTRes : class, new()
        {
            var taskResults =
                tasks.ToDictionary(t => t, t => GroupExecutorUtil.TryGetTaskItemResult(data, t));

            await Task.WhenAll(taskResults.Select(tr => tr.Value));


            var taskResps = taskResults.ToDictionary(d => d.Key, d => d.Value.Result);

            nodeResp.TaskResults = taskResps.ToDictionary(tk => tk.Key.Meta, tk => tk.Value);

            return taskResps.Select(tItemRes 
                => GroupExecutorUtil.FormatNodeErrorResp(tItemRes.Value, tItemRes.Key.Meta))
                .Aggregate<GroupExecuteStatus, GroupExecuteStatus>(0, (current, s) => current | s);
        }

        // 并行任务回退处理（回退当前其他所有任务）
        internal static async Task Executing_ParallelRevert<TTData, TTRes>(this EventTask.GroupEventTask<TTData, TTRes> node,
            TTData data, GroupEventTaskResp<TTRes> nodeResp, IList<IEventTask<TTData, TTRes>> tasks, string blockTaskId)
            where TTData : class where TTRes : class, new()
        {
            var revResList = tasks.Select(tItem => tItem.Meta.task_id == blockTaskId
                    ? Task.FromResult(true)
                    : GroupExecutorUtil.TryRevertTask(tItem, data))
                .ToArray();

            await Task.WhenAll(revResList);


            if (nodeResp.RevrtTasks == null)
                nodeResp.RevrtTasks = new List<TaskMeta>(tasks.Count);

            for (var i = 0; i < tasks.Count; i++)
            {
                var res = false;
                var resT = revResList[i];

                if (resT.Status == TaskStatus.RanToCompletion)
                    res = resT.Result;

                if (res)
                    nodeResp.RevrtTasks.Add(tasks[i].Meta);
            }
        }
    }
}