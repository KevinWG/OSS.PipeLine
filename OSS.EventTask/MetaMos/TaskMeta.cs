using OSS.EventTask.Mos;

namespace OSS.EventTask.MetaMos
{
    public class TaskMeta
    {
        public string flow_id { get; set; }
       
        public string node_id { get; set; }

        /// <summary>
        ///  任务键
        /// </summary>
        public string task_id { get; set; }

        /// <summary>
        ///  任务名称
        /// </summary>
        public string task_alias { get; set; }

        /// <summary>
        ///   归属类型
        /// </summary>
        public OwnerType owner_type { get; set; }

        ///// <summary>
        ///// 当前状态
        ///// </summary>
        //public TaskMetaStatus status { get; set; }

        /// <summary>
        ///  结果动作
        /// </summary>
        public NodeResultAction node_action { get; set; }

        /// <summary>
        ///  直接循环次数
        /// </summary>
        public int loop_times { get; set; } = 1;

        /// <summary>
        ///   重试运行次数,默认不重试运行
        /// </summary>
        public int retry_times { get; set; } = 0;

        /// <summary>
        ///  重试的间隔时长（秒）
        /// </summary>
        public int retry_seconds { get; set; } = 0;
    }

    public enum NodeResultAction
    {
        /// <summary>
        /// 失败后继续
        /// </summary>
        ContinueAnyway=0,

        /// <summary>
        ///   当前任务失败后节点暂停
        /// </summary>
        PauseOnFailed = 10,

        /// <summary>
        ///   当前任务失败后整个节点执行失败
        /// </summary>
        FailedOnFailed = 20,

        /// <summary>
        ///   失败后回退所有已执行任务
        /// </summary>
        FailedRevrtOnFailed = 30,
    }

}
