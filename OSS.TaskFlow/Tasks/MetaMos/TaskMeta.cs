namespace OSS.TaskFlow.Tasks.MetaMos
{
    public class TaskMeta
    {
        /// <summary>
        ///  任务键
        /// </summary>
        public string task_key { get; set; }

        /// <summary>
        ///  任务名称
        /// </summary>
        public string task_name { get; set; }

        public string node_key { get; set; }

        public string flow_key { get; set; }

        public TaskMetaStatus status { get; set; }

        public RunType run_type { get; set; }

    }


    public enum TaskMetaStatus
    {
        Delete=-1000,
        Disable=-1,
        Enable=0
    }
    public enum RunType
    {
        /// <summary>
        ///   当前任务失败后后续任务中断
        /// </summary>
        FailedBreak=0,


        /// <summary>
        /// 
        /// </summary>
        ContinueOnFailed
    }

}
