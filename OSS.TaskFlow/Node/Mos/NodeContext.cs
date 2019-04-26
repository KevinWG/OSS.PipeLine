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
}
