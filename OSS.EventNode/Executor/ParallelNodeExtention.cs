﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.EventNode.Mos;
using OSS.EventTask.Interfaces;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;

namespace OSS.EventNode.Executor
{
    public static class ParallelNodeExtention
    {
        ///   并行执行
       internal static void Excuting_Parallel<TTReq, TTRes>(this BaseNode<TTReq, TTRes> node, TTReq req, NodeResponse<TTRes> nodeResp,
            IList<IBaseTask<TTReq>> tasks)
            where TTReq : ExcuteReq
            where TTRes : ResultMo, new()
        {
            var taskResults =
                tasks.ToDictionary(t => t, t => ExecutorUtil.TryGetTaskItemResult(req, t, new RunCondition(),node.InstanceNodeType));

            try
            {
                var tw = Task.WhenAll(taskResults.Select(tr => tr.Value));
                tw.Wait();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex, node.NodeMeta.node_id, node.ModuleName);
            }

            var taskResps = taskResults.ToDictionary(d => d.Key, d => d.Value.Status == TaskStatus.Faulted
                ? new TaskResponse<ResultMo>().WithError(TaskRunStatus.RunFailed, new RunCondition())
                : d.Value.Result);

         
            nodeResp.node_status = NodeStatus.ProcessCompoleted; // 循环里会处理结果，这里给出最大值

            foreach (var tItemRes in taskResps)
            {
                ExecutorUtil.FormatNodeErrorResp(nodeResp, tItemRes.Value, tItemRes.Key.TaskMeta);
                if (nodeResp.node_status == NodeStatus.ProcessFailedRevert)
                {
                    Excuting_ParallelRevert(node,req, nodeResp, tasks);
                    break;
                }
            }
        }



        //  顺序任务的回退处理
        private static async void Excuting_ParallelRevert<TTReq, TTRes>(BaseNode<TTReq, TTRes> node, TTReq req, NodeResponse<TTRes> nodeResp,
            IList<IBaseTask<TTReq>> tasks)
            where TTReq : ExcuteReq
            where TTRes : ResultMo, new()
        {
           var revResList =  tasks.Select(tItem => ExecutorUtil.TryRevertTask(tItem, req)).ToArray();

            try
            {
                var tw = Task.WhenAll(revResList);
                tw.Wait();
            }
            catch (Exception ex)
            {
                LogUtil.Error("An error occurred while the parallel node reverted all tasks. Detail:{ex}  ", 
                    node.NodeMeta.node_id, node.ModuleName);
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                bool res = false;
                var resT = revResList[i];
               
                if (resT.Status==TaskStatus.RanToCompletion)
                    res = resT.Result;


                if (res)
                 nodeResp.RevrtTasks.Add(tasks[i].TaskMeta);
            }
        }



    }
}