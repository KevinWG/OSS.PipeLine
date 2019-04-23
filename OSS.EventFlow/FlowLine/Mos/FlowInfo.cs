namespace OSS.EventFlow.FlowLine.Mos
{
    public class FlowInfo
    {
        /// <summary>
        ///  当前流程Id
        /// </summary>
        public string flow_id { get; set; }

        /// <summary>
        /// 流程编号
        /// </summary>
        public string flow_code { get; set; }

        /// <summary>
        ///  所在流程节点
        /// </summary>
        public string at_node_code { get; set; }

        /// <summary>
        ///  经过的节点数量
        /// </summary>
        public int passed_node { get; set; }
    }
}
