namespace OSS.TaskFlow
{
    //public abstract class TaskEngine
    //{

    //    public void Excute(ExcuteReq req)
    //    {
    //    }

    //    public abstract void AddFlow();
    //    public abstract void AddNode();
    //    public abstract void AddTask();
    //}

    /// <summary>
    ///   执行请求
    /// </summary>
    public class ExcuteReq
    {
        public string flow_key { get; set; }

        /// <summary>
        ///   请求内容体
        /// </summary>
        public object body { get; set; }

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
