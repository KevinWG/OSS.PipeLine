using OSS.Common.ComModels;
using System.Collections.Generic;
using System.Linq;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode.Mos
{

    public class NodeResponse<TRes>
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

        /// <summary>
        ///   节点内回退节点信息
        /// </summary>
        public IList<TaskMeta> RevrtTasks { get; internal set; }

        /// <summary>
        ///  节点任务处理结果
        /// </summary>
        public IDictionary<TaskMeta, TaskResponse<ResultMo>> TaskResults { get;internal set; }
        
        /// <summary>
        ///  获取对应TaskKey对应的任务结果
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public TaskResponse<ResultMo> this[string taskId] =>
            (from taskRes in TaskResults where taskRes.Key.task_id == taskId select taskRes.Value).FirstOrDefault();
    }
}
