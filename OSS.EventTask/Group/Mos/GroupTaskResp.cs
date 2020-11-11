using System;
using System.Collections.Generic;
using System.Linq;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask.Group.Mos
{
    [Flags]
    public enum GroupExecuteStatus
    {
        Complete = 1,
        Failed = 2,
        Revert = 4
    }

    public class GroupTaskResp<TRes> :BaseTaskResp<TRes>
           where TRes : class, new()
    {

        /// <summary>
        ///   节点内回退节点信息
        /// </summary>
        public IList<TaskMeta> RevrtTasks { get; internal set; }

        /// <summary>
        ///  当前阻断执行的任务Id
        /// </summary>
        public string block_taskid{ get; set; }

        /// <summary>
        ///  节点任务处理结果
        /// </summary>
        public IDictionary<TaskMeta, TaskResp<TRes>> TaskResults { get; internal set; }

        /// <summary>
        ///  获取对应TaskKey对应的任务结果
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public TaskResp<TRes> this[string taskId] =>
            (from taskRes in TaskResults where taskRes.Key.task_id == taskId select taskRes.Value).FirstOrDefault();
    }
}
