using OSS.Common.ComModels;
using System.Collections.Generic;
using System.Linq;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode.Mos
{
    public class ProcessResp<TRes>
           where TRes : ResultMo, new()
    {
        /// <summary>
        ///  最终结果
        /// </summary>
        public TRes resp { get; set; }

        /// <summary>
        ///  节点状态
        /// </summary>
        public NodeStatus node_status { get; set; }
    }
    
    public class NodeResp<TRes> 
           where TRes : ResultMo, new()
    {
        /// <summary>
        ///  最终结果
        /// </summary>
        public TRes resp { get; internal set; }

        /// <summary>
        ///  节点状态
        /// </summary>
        public NodeStatus node_status { get; internal set; }

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
        public IDictionary<TaskMeta, TaskResp<ResultMo>> TaskResults { get; internal set; }

        /// <summary>
        ///  获取对应TaskKey对应的任务结果
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public TaskResp<ResultMo> this[string taskId] =>
            (from taskRes in TaskResults where taskRes.Key.task_id == taskId select taskRes.Value).FirstOrDefault();
    }
}
