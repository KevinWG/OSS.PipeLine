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
        internal static async Task<GroupExecuteResp<TTData, TTRes>> Executing_Parallel<TTData, TTRes>(this IList<IEventTask<TTData, TTRes>> tasks, TTData data)
            where TTData : class
            //where TTRes : class, new()
        {
            var taskHandlers =
                tasks.ToDictionary(t => t, t => GroupExecutorUtil.TryGetTaskItemResult(data, t));

            await Task.WhenAll(taskHandlers.Select(tr => tr.Value));


            var taskResps = taskHandlers.ToDictionary(d => d.Key, d => d.Value.Result);

            GroupExecuteStatus exeStatus = 0;
            foreach (var eventTaskResp in taskResps)
            {
                var s = GroupExecutorUtil.FormatEffectStatus(eventTaskResp.Value);
                exeStatus |= s;
            }

            return new GroupExecuteResp<TTData, TTRes>()
            {
                status = exeStatus,
                TaskResults = taskResps
            };
        }

        // 并行任务回退处理（回退当前其他所有任务）
        internal static async Task Executing_ParallelRevert<TTData, TTRes>( this IDictionary<IEventTask<TTData, TTRes>, EventTaskResp<TTRes>> taskResults, TTData data)
            where TTData : class 
            //where TTRes : class, new()
        {
            var revertTaskHandlers=new List<Task>();
         
            foreach (var taskPair in taskResults)
            {
                var res = taskPair.Value;

                if (res.run_status == TaskRunStatus.RunCompleted
                    && res.meta.revert_effect == RevertEffect.RevertGroup && !res.has_reverted)
                {
                    revertTaskHandlers.Add(taskPair.Key.Revert(data));
                }
            }

            if (revertTaskHandlers.Count>0)
            {
                await Task.WhenAll(revertTaskHandlers);
            }
     
        }
    }
}