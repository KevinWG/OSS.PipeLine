using OSS.Common.ComModels;
using OSS.EventNode.MetaMos;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode.Mos
{
    public class NodeContext<TDomain,TReq,TRes> : NodeContext<TRes>
        where TRes : ResultMo, new()
    {
        /// <summary>
        ///  请求信息
        /// </summary>
        public BaseReq<TDomain, TReq> req { get; set; }
    }

    /// <summary>
    ///   请求数据
    /// </summary>
    /// <typeparam name="TReq"></typeparam>
    /// <typeparam name="TRes"></typeparam>
    public class NodeContext<TReq,TRes> : NodeContext<TRes>
        where TRes : ResultMo, new()
    {
        /// <summary>
        ///  请求信息
        /// </summary>
        public BaseReq<TReq> req { get; set; }
    }

    public abstract class NodeContext<TRes>
        where TRes : ResultMo, new()
    {
        /// <summary>
        ///  当前流-节点元信息
        /// </summary>
        public NodeMeta node_meta { get; set; }

        /// <summary>
        ///  结果
        /// </summary>
        public TRes resp { get; set; }

        /// <summary>
        ///  节点状态
        /// </summary>
        public NodeStatus node_status { get; set; }
    }

    public static class NodeContextExtention
    {
        public static TaskContext<TReq,ResultMo> ConvertToTaskContext<TReq,TRes>(this NodeContext<TReq, TRes> nodeContext,TaskMeta taskMeta)
        where TRes:ResultMo,new ()
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


        public static TaskContext<TDomain, TReq,ResultMo> ConvertToTaskContext<TDomain,TReq, TRes>(this NodeContext<TDomain,TReq, TRes> nodeContext,
            TaskMeta taskMeta) where TRes : ResultMo, new()
        {
            var taskContext = new TaskContext<TDomain, TReq,  ResultMo>();
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
