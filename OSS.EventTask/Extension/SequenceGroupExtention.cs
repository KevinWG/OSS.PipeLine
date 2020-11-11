using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask.Extension
{
    public static class SerialGroupExtension
    {
        ///  顺序执行
        internal static async Task<GroupExecuteStatus> Executing_Serial<TTData, TTRes>(this EventTask.GroupEventTask<TTData, TTRes> node,
            TTData data,GroupEventTaskResp<TTRes> nodeResp, IList<IEventTask<TTData, TTRes>> tasks)
            where TTData : class where TTRes : class, new()
        {
            nodeResp.TaskResults = new Dictionary<TaskMeta, TaskResp<TTRes>>(tasks.Count);
      
            foreach (var tItem in tasks)
            {
                var taskResp = await GroupExecutorUtil.TryGetTaskItemResult(data, tItem);

                var tMeta = tItem.Meta;
                nodeResp.TaskResults.Add(tMeta, taskResp);

                var exeStatus = GroupExecutorUtil.FormatNodeErrorResp(taskResp, tMeta);
                if ((exeStatus&GroupExecuteStatus.Failed)==GroupExecuteStatus.Failed)
                {
                    nodeResp.block_taskid = tMeta.task_id;
                    return exeStatus;
                }
            }

            return GroupExecuteStatus.Complete;
        }


        //  顺序任务 回退当前任务之前所有任务
        internal static async Task Executing_SerialRevert<TTData, TTRes>(this EventTask.GroupEventTask<TTData, TTRes> node,TTData data, GroupEventTaskResp<TTRes> nodeResp,
            IList<IEventTask<TTData, TTRes>> tasks,string blockTaskId)
            where TTData : class where TTRes : class, new()
        {
            if (nodeResp.RevrtTasks==null)
                nodeResp.RevrtTasks=new List<TaskMeta>(tasks.Count);
            
            foreach (var tItem in tasks)
            {
                if (tItem.Meta.task_id== blockTaskId)
                {
                    nodeResp.RevrtTasks.Add(tItem.Meta);
                    break;
                }

                var rRes = await GroupExecutorUtil.TryRevertTask(tItem, data);// tItem.Revert(data);
                if (rRes)
                    nodeResp.RevrtTasks.Add(tItem.Meta);
            }
        }



      

    }
}
