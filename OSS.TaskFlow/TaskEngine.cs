namespace OSS.TaskFlow
{
    public abstract class TaskEngine
    {

        public void Excute(ExcuteReq req)
        {

        }

        public abstract void AddFlow();
        public abstract void AddNode();
        public abstract void AddTask();
    }



    public class ExcuteReq
    {
        public string flow_key { get; set; }

        /// <summary>
        ///   请求内容体
        /// </summary>
        public object body { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string node_key { get; set; }

        public string task_key { get; set; }
    }
}
