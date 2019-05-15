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
        internal static async Task Excuting_Parallel<TTReq, TTRes>(this BaseNode<TTReq, TTRes> node, TTReq req,
            NodeResponse<TTRes> nodeResp,
            IList<IBaseTask<TTReq>> tasks)
            where TTReq : class
            where TTRes : ResultMo, new()
        {
            var taskResults =
                tasks.ToDictionary(t => t, t => ExecutorUtil.TryGetTaskItemResult(req, t, new RunCondition()));

            try
            {
                await Task.WhenAll(taskResults.Select(tr => tr.Value));
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex, node.NodeMeta.node_id, node.ModuleName);
            }

            var taskResps = taskResults.ToDictionary(d => d.Key, d => d.Value.Status == TaskStatus.Faulted
                ? new TaskResponse<ResultMo>().WithError(TaskRunStatus.RunFailed, new RunCondition())
                : d.Value.Result);

            nodeResp.TaskResults = new Dictionary<TaskMeta, TaskResponse<ResultMo>>(tasks.Count);
            nodeResp.node_status = NodeStatus.ProcessCompoleted; // 循环里会处理结果，这里给出最大值

            foreach (var tItemRes in taskResps)
            {
                nodeResp.TaskResults.Add(tItemRes.Key.TaskMeta, tItemRes.Value);
                ExecutorUtil.FormatNodeErrorResp(nodeResp, tItemRes.Value, tItemRes.Key.TaskMeta);

                if (nodeResp.node_status == NodeStatus.ProcessFailedRevert)
                {
                    await Excuting_ParallelRevert(node, req, nodeResp, tasks, tItemRes.Key.TaskMeta);
                    break;
                }
            }
        }



        // 并行任务回退处理
        private static async Task Excuting_ParallelRevert<TTReq, TTRes>(BaseNode<TTReq, TTRes> node, TTReq req,
            NodeResponse<TTRes> nodeResp,
            IList<IBaseTask<TTReq>> tasks, TaskMeta errorTask)
            where TTReq : class
            where TTRes : ResultMo, new()
        {
            var revResList = tasks.Select(tItem => tItem.TaskMeta.task_id== errorTask.task_id
                    ? Task.FromResult(true)
                    : ExecutorUtil.TryRevertTask(tItem, req))
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
