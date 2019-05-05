namespace OSS.EventFlow.Mos
{
    public class FlowReq
    {
        /// <summary>
        /// 
        /// </summary>
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
        ///   请求内容体
        /// </summary>
        public object body { get; set; }


        /// <summary>
        ///  流Id
        /// </summary>
        public string exc_id { get; set; }
    }
}
