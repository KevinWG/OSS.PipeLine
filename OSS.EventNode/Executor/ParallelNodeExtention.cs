using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.EventNode.Mos;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;

namespace OSS.EventNode.Executor
{
    public static class ParallelNodeExtention
    {
        //   并行执行任务扩展
        internal static async Task Excuting_Parallel<TTData, TTRes>(this GroupEventTask<TTData, TTRes> node, TTData data,
            GroupTaskResp<TTRes> nodeResp, IList<IEventTask<TTData, TTRes>> tasks, int triedTimes)
            where TTData : class
            where TTRes : class, new()
        {
            var taskResults =
                tasks.ToDictionary(t => t, t => ExecutorUtil.TryGetTaskItemResult(data, t, triedTimes));

            //try
            //{
            await Task.WhenAll(taskResults.Select(tr => tr.Value));
            //}
            //catch (Exception ex)
            //{
            //    LogUtil.Error(ex, node.NodeMeta.node_id, EventTaskProvider.ModuleName);
            //}

            var taskResps = taskResults.ToDictionary(d => d.Key, d => d.Value.Result);

            nodeResp.TaskResults = taskResps.ToDictionary(tk => tk.Key.Meta, tk => tk.Value);
            nodeResp.node_status = NodeStatus.ProcessCompoleted; // 循环里会处理结果，这里给出最大值

            foreach (var tItemRes in taskResps)
            {
                var isBlocked = ExecutorUtil.FormatNodeErrorResp(nodeResp, tItemRes.Value, tItemRes.Key.Meta);
                if (isBlocked)
                {
                    nodeResp.block_taskid = tItemRes.Key.Meta.task_id;
                    if (nodeResp.node_status == NodeStatus.ProcessFailedRevert)
                        break;
                }
            }
        }

        // 并行任务回退处理（回退当前其他所有任务）
        internal static async Task Excuting_ParallelRevert<TTData, TTRes>(this GroupEventTask<TTData, TTRes> node,
            TTData data, GroupTaskResp<TTRes> nodeResp, IList<IEventTask<TTData, TTRes>> tasks, string blockTaskId)
            where TTData : class where TTRes : class, new()
        {
            var revResList = tasks.Select(tItem => tItem.Meta.task_id == blockTaskId
                    ? Task.FromResult(true)
                    : ExecutorUtil.TryRevertTask(tItem, data))
                .ToArray();
            //try
            //{
            await Task.WhenAll(revResList);
            //}
            //catch (Exception ex)
            //{
            //    LogUtil.Error($"An error occurred while the parallel node reverted all tasks. Detail:{ex}",
            //        node.NodeMeta.node_id, EventTaskProvider.ModuleName);
            //}

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
