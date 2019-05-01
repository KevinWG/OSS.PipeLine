using OSS.EventNode.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode.Mos
{
    public class NodeContext : TaskContext
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
