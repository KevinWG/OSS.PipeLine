using OSS.EventNode.MetaMos;
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


    public class NodeContext :BaseContext
    {
        /// <summary>
        ///  当前流-节点元信息
        /// </summary>
        public NodeMeta node_meta { get; set; }
    }

    //public static class NodeContextExtention
    //{
    //    public static NodeContext ConvertToTaskContext(this FlowContext node)
    //    {
    //        var nodeCon = new NodeContext
    //        {
    //            run_id = node.run_id,
    //            flow_meta = node.flow_meta
    //        };
    //        return nodeCon;
    //    }
    //}

}
