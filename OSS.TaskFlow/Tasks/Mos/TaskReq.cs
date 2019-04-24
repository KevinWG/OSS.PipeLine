namespace OSS.TaskFlow.Tasks.Mos
{
    public class TaskReq<TReq>
    {
        /// <summary>
        /// 事件内容体
        /// </summary>
        public TReq Body { get; set; }
    }
}
