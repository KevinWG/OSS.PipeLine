namespace OSS.TaskFlow.Tasks.Mos
{
    public class TaskReq
    {
        /// <summary>
        /// 
        /// </summary>
        public string FlowId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NodeCode { get; set; }

        /// <summary>
        /// 事件内容体
        /// </summary>
        public dynamic Body { get; set; }

    }
}
