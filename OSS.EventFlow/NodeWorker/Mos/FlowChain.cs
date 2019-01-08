using System.Collections.Generic;

namespace OSS.EventFlow.NodeWorker.Mos
{
    /// <summary>
    ///  节点流链条
    /// </summary>
    public class NodeFlowChain : NodeInfo
    {
        public NodeInfo Next { get; set; }
    }
}
