namespace OSS.TaskFlow.Node.Mos
{
    public class NodeReq
    {
        public string flow_key { get; set; }

        /// <summary>
        /// 节点Key
        /// </summary>
        public string node_key { get; set; }

        /// <summary>
        ///   任务key
        /// </summary>
        public string task_key { get; set; }

        /// <summary>
        ///  流Id
        /// </summary>
        public string flow_id { get; set; }
    }
}
