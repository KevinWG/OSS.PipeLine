using OSS.TaskFlow.FlowLine.Mos;
using OSS.TaskFlow.Node.MetaMos;

namespace OSS.TaskFlow.Node.Mos
{
    public class NodeContext:FlowContext
    {
        /// <summary>
        ///  当前流-节点元信息
        /// </summary>
        public NodeMeta NodeMeta { get; set; }
    }

    public static class NodeContextExtention
    {
        public static NodeContext ConvertToTaskContext(this FlowContext node)
        {
            var nodeCon = new NodeContext
            {
                run_id = node.run_id,
                FlowMeta = node.FlowMeta
            };
            return nodeCon;
        }
    }

}
