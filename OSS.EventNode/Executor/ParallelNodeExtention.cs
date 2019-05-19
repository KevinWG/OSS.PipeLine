using System;
using System.Collections.Generic;
using System.Linq;
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
    public static class ParallelNodeExtention
    {
        //   并行执行任务扩展
        internal static async Task Excuting_Parallel<TTData, TTRes>(this BaseNode<TTData, TTRes> node,TTData data,
            NodeResp<TTRes> nodeResp,IList<IEventTask<TTData>> tasks,int triedTimes)
            where TTData : class
            where TTRes : ResultMo, new()
        {
            var taskResults =
                tasks.ToDictionary(t => t, t => ExecutorUtil.TryGetTaskItemResult(data, t, triedTimes));

            try
            {
                await Task.WhenAll(taskResults.Select(tr => tr.Value));
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex, node.NodeMeta.node_id, node.ModuleName);
            }

            var taskResps = taskResults.ToDictionary(d => d.Key, d => d.Value.Status == TaskStatus.Faulted
                ? new TaskResp<ResultMo>().WithError(TaskRunStatus.RunFailed, new RunCondition())
                : d.Value.Result);

            nodeResp.TaskResults = taskResps.ToDictionary(tk => tk.Key.TaskMeta, tk => tk.Value);
            nodeResp.node_status = NodeStatus.ProcessCompoleted; // 循环里会处理结果，这里给出最大值

            foreach (var tItemRes in taskResps)
            {
                var isBlocked = ExecutorUtil.FormatNodeErrorResp(nodeResp, tItemRes.Value, tItemRes.Key.TaskMeta);
                if (isBlocked)
                {
                    nodeResp.block_taskid = tItemRes.Key.TaskMeta.task_id;
                    if (nodeResp.node_status == NodeStatus.ProcessFailedRevert)
                        break;
                }
            }
        }
        
        // 并行任务回退处理（回退当前其他所有任务）
        internal static async Task Excuting_ParallelRevert<TTData, TTRes>(this BaseNode<TTData, TTRes> node,
            TTData data,NodeResp<TTRes> nodeResp,IList<IEventTask<TTData>> tasks,string blockTaskId,int triedTimes)
            where TTData : class where TTRes : ResultMo, new()
        {
            var revResList = tasks.Select(tItem => tItem.TaskMeta.task_id== blockTaskId
                    ? Task.FromResult(true)
                    : ExecutorUtil.TryRevertTask(tItem, data, triedTimes))
                .ToArray();
            try
            {
                await Task.WhenAll(revResList);
            }
            catch (Exception ex)
            {
                LogUtil.Error($"An error occurred while the parallel node reverted all tasks. Detail:{ex}",
                    node.NodeMeta.node_id, node.ModuleName);
            }

            if (nodeResp.RevrtTasks == null)
                nodeResp.RevrtTasks = new List<TaskMeta>(tasks.Count);

            for (var i = 0; i < tasks.Count; i++)
            {
                var res = false;
                var resT = revResList[i];

                if (resT.Status == TaskStatus.RanToCompletion)
                    res = resT.Result;

                if (res)
                    nodeResp.RevrtTasks.Add(tasks[i].TaskMeta);
            }
        }



    }
}
