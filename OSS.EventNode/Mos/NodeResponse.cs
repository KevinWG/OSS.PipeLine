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
        public IDictionary<TaskMeta, TaskResponse<ResultMo>> TaskResults { get; set; }

       public TaskResponse<ResultMo> this[string taskKey] => (from taskRes in TaskResults where taskRes.Key.task_key == taskKey select taskRes.Value).FirstOrDefault();

        /// <summary>
        ///  结果
        /// </summary>
        public TRes resp { get; set; }

        /// <summary>
        ///  节点状态
        /// </summary>
        public NodeStatus node_status { get; set; }

        /// <summary>
        /// 阻断任务数量
        /// </summary>
        public int pause_task_count { get; set; }
    }

    public static class NodeContextExtention
    {
        //public static TaskResponse<TReq,ResultMo> ConvertToTaskContext<TReq,TRes>(this NodeResponse<TReq, TRes> nodeContext,TaskMeta taskMeta)
        //where TRes:ResultMo,new ()
        //{
        //    //var taskContext = new TaskContext<TReq>
        //    //{
        //    //    flow_key = nodeContext.flow_key,
        //    //    node_key = nodeContext.node_meta.node_key,
        //    //    exc_id = nodeContext.exc_id,

        //    //    req_data = nodeContext.req_data,
        //    //    task_meta = taskMeta
        //    //};
        //    return new TaskResponse<TReq,ResultMo>();
        //}


        //public static TaskResponse<TDomain, TReq,ResultMo> ConvertToTaskContext<TDomain,TReq, TRes>(this NodeResponse<TDomain,TReq, TRes> nodeContext,
        //    TaskMeta taskMeta) where TRes : ResultMo, new()
        //{
        //    var taskContext = new TaskResponse<TDomain, TReq,  ResultMo>();
        //    //{
        //    //    flow_key = nodeContext.flow_key,
        //    //    node_key = nodeContext.node_meta.node_key,
        //    //    exc_id = nodeContext.exc_id,
        //    //    req_data = nodeContext.req_data,

        //    //    task_meta = taskMeta,
        //    //    domain_data =  nodeContext.domain_data
        //    //};
        //    return taskContext;

        //}
    }

}
