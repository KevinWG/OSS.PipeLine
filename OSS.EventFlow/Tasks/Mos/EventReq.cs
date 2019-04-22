namespace OSS.EventFlow.Tasks.Mos
{
    public class EventReq
    {

        public string ContextId { get; set; }

        public string FlowCode { get; set; }

        public string NodeCode { get; set; }
        
        /// <summary>
        /// 事件内容体
        /// </summary>
        public dynamic Body { get; set; }

    }
}
