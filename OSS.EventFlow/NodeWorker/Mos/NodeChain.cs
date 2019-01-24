namespace OSS.EventFlow.NodeWorker.Mos
{
    /// <summary>
    ///  节点流链条
    /// </summary>
    public class NodeChain : NodeInfo
    {
        public NodeInfo Next { get; set; }
    }
}
