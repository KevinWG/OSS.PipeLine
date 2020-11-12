#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventTask - 事件任务辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2019-04-07
*       
*****************************************************************************/

#endregion
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
        internal static GroupExecuteStatus FormatEffectStatus<TTRes>(EventTaskResp<TTRes> taskResp)
            //where TTRes : class, new()
        {
            if (taskResp.meta.failed_effect != FailedEffect.FailedGroup || taskResp.run_status.IsCompleted())
            {
                return GroupExecuteStatus.Complete;
            }

            if (taskResp.meta.revert_effect == RevertEffect.RevertGroup)
            {
                return GroupExecuteStatus.Failed | GroupExecuteStatus.Revert;
            }

            return GroupExecuteStatus.Failed;
        }

        //  尝试获取任务执行结果
        internal static Task<EventTaskResp<TTRes>> TryGetTaskItemResult<TTData,TTRes>(TTData data, IEventTask<TTData, TTRes> task)
            where TTData : class
            //where TTRes : class, new()
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

  


     

    }
}
