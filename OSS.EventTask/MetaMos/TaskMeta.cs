namespace OSS.EventTask.MetaMos
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
        
        /// <summary>
        /// 
        /// </summary>
        public TaskMetaStatus status { get; set; }
        
        /// <summary>
        ///  运行类型
        /// </summary>
        public RunType run_type { get; set; }
        
        /// <summary>
        ///  直接重试次数，默认系统执行一次
        /// </summary>
        public int continue_times { get; set; }

        /// <summary>
        ///  间隔重试次数, 默认不会执行
        /// </summary>
        public int interval_times { get; set; }
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
        /// 
        /// </summary>
        ContinueOnFailed=0,

        /// <summary>
        ///   当前任务失败后后续任务暂停
        /// </summary>
        PauseOnFailed = 10,

        /// <summary>
        ///   失败后回退所有已执行任务
        /// </summary>
        RevrtAllOnFailed = 20,
    }

}
