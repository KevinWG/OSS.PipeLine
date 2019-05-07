using OSS.Common.ComModels;
using OSS.EventNode.MetaMos;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode.Mos
{
    public class NodeContext<TReq, TDomain> : NodeContext<TReq>
    {
        /// <summary>
        ///   核心流数据
        /// </summary>
        public TDomain domain_data { get; set; }
    }

    /// <summary>
    ///   请求数据
    /// </summary>
    /// <typeparam name="TReq"></typeparam>
    public class NodeContext<TReq> : NodeContext
    {
        /// <summary>
        ///   执行请求内容主体
        /// </summary>
        public TReq req_data { get; set; }
    }


    public class NodeContext 
    {
        /// <summary>
        ///  当前流-节点元信息
        /// </summary>
        public NodeMeta node_meta { get; set; }
    }

    public static class NodeContextExtention
    {
        public static TaskContext<TReq,ResultMo> ConvertToTaskContext<TReq>(this NodeContext<TReq> nodeContext,TaskMeta taskMeta)
        {
            //var taskContext = new TaskContext<TReq>
            //{
            //    flow_key = nodeContext.flow_key,
            //    node_key = nodeContext.node_meta.node_key,
            //    exc_id = nodeContext.exc_id,

            //    req_data = nodeContext.req_data,
            //    task_meta = taskMeta
            //};
            return new TaskContext<TReq,ResultMo>();
        }


        public static TaskContext<TReq, TDomain,ResultMo> ConvertToTaskContext<TReq, TDomain>(this NodeContext<TReq, TDomain> nodeContext,
            TaskMeta taskMeta)
        {
            var taskContext = new TaskContext<TReq, TDomain, ResultMo>();
            //{
            //    flow_key = nodeContext.flow_key,
            //    node_key = nodeContext.node_meta.node_key,
            //    exc_id = nodeContext.exc_id,
            //    req_data = nodeContext.req_data,

            //    task_meta = taskMeta,
            //    domain_data =  nodeContext.domain_data
            //};
            return taskContext;

        }
    }

}
