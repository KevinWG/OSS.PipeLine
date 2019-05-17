using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventNode.Mos;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode.Executor
{
    public static class SequenceNodeExtention
    {
        ///  顺序执行
        internal static async Task Excuting_Sequence<TTData, TTRes>(this BaseNode<TTData, TTRes> node,
            TTData req,NodeResponse<TTRes> nodeResp, IList<IBaseTask<TTData>> tasks,int triedTimes)
            where TTData : class where TTRes : ResultMo, new()
        {
            nodeResp.TaskResults = new Dictionary<TaskMeta, TaskResponse<ResultMo>>(tasks.Count);
            nodeResp.node_status = NodeStatus.ProcessCompoleted; // 默认成功，给出最大值，循环内部处理
            
            foreach (var tItem in tasks)
            {
                var taskResp = await ExecutorUtil.TryGetTaskItemResult(req, tItem, triedTimes);

                var tMeta = tItem.TaskMeta;
                nodeResp.TaskResults.Add(tMeta, taskResp);

                var haveError = ExecutorUtil.FormatNodeErrorResp(nodeResp, taskResp, tMeta);
                if (haveError)
                {
                    nodeResp.block_taskid = tMeta.task_id;
                    break;
                }
            }
        }


        //  顺序任务 回退当前任务之前所有任务
        internal static async Task Excuting_SequenceRevert<TTData, TTRes>(this BaseNode<TTData, TTRes> node,TTData req, NodeResponse<TTRes> nodeResp,
            IList<IBaseTask<TTData>> tasks,string blockTaskId,int triedTimes)
            where TTData : class where TTRes : ResultMo, new()
        {
            if (nodeResp.RevrtTasks==null)
                nodeResp.RevrtTasks=new List<TaskMeta>(tasks.Count);
            
            foreach (var tItem in tasks)
            {
                if (tItem.TaskMeta.task_id== blockTaskId)
                {
                    nodeResp.RevrtTasks.Add(tItem.TaskMeta);
                    break;
                }

                var rRes = await ExecutorUtil.TryRevertTask(tItem, req, triedTimes);// tItem.Revert(req);
                if (rRes)
                    nodeResp.RevrtTasks.Add(tItem.TaskMeta);
            }
        }



      

    }
}
