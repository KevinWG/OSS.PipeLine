namespace OSS.EventFlow.Mos
{
    public class FlowReq
    {
        /// <summary>
        /// 
        /// </summary>
        public string flow_id { get; set; }

        /// <summary>
        /// 节点Key
        /// </summary>
        public string node_id { get; set; }

        /// <summary>
        ///   任务key
        /// </summary>
        public string task_id { get; set; }

        /// <summary>
        ///   请求内容体
        /// </summary>
        public object body { get; set; }


        /// <summary>
        ///  流Id
        /// </summary>
        public string exe_id { get; set; }
    }
}
