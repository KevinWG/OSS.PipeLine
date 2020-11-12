#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventTask - 群组事件任务串行处理扩展
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2019-04-07
*       
*****************************************************************************/

#endregion


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
        internal static async Task<GroupExecuteResp<TTData, TTRes>> Executing_Serial<TTData, TTRes>(this IList<IEventTask<TTData, TTRes>> tasks, TTData data)
            where TTData : class 
            //where TTRes : class, new()
        {
            var exeResp = new GroupExecuteResp<TTData, TTRes>
            {
                TaskResults = new Dictionary<IEventTask<TTData, TTRes>, EventTaskResp<TTRes>>(tasks.Count)
            };

            foreach (var tItem in tasks)
            {
                var taskResp = await GroupExecutorUtil.TryGetTaskItemResult(data, tItem);

                exeResp.TaskResults.Add(tItem, taskResp);
                exeResp.status |= GroupExecutorUtil.FormatEffectStatus(taskResp);

                if ((exeResp.status & GroupExecuteStatus.Failed) == GroupExecuteStatus.Failed)
                {
                    return exeResp;
                }
            }

            return exeResp;
        }


        //  顺序任务 回退当前任务之前所有任务
        internal static async Task Executing_SerialRevert<TTData, TTRes>(this IDictionary<IEventTask<TTData, TTRes>, EventTaskResp<TTRes>> taskResults, TTData data)
            where TTData : class
            //where TTRes : class, new()
        {
            var revertTaskHandlers = new List<IEventTask<TTData, TTRes>>();
            foreach (var taskPair in taskResults)
            {
                var res = taskPair.Value;

                if (res.run_status == TaskRunStatus.RunCompleted
                    && res.meta.revert_effect == RevertEffect.RevertGroup && !res.has_reverted)
                {
                    revertTaskHandlers.Add(taskPair.Key);
                }
            }

            // 倒序回退
            for (var i = revertTaskHandlers.Count - 1; i >= 0; i--)
            {
                var task = revertTaskHandlers[i];
                await task.Revert(data);
            }
        }
    }
}
