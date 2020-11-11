using OSS.EventTask.Mos;

namespace OSS.EventTask.MetaMos
{
    public class BaseTaskMeta
    {
        /// <summary>
        ///  流程Id
        /// </summary>
        public string flow_id { get; set; }

        /// <summary>
        ///  分组ID
        /// </summary>
        public string group_id { get; set; }

        /// <summary>
        ///  任务键
        /// </summary>
        public string task_id { get; set; }

        /// <summary>
        ///   重试运行次数,默认不重试运行
        /// </summary>
        public int retry_times { get; set; } = 0;

        /// <summary>
        ///  重试的间隔时长（秒）
        /// </summary>
        public int retry_seconds { get; set; } = 0;
    }

    public class TaskMeta : BaseTaskMeta
    {
        /// <summary>
        ///  任务名称
        /// </summary>
        public string task_alias { get; set; }

        /// <summary>
        ///   归属类型
        ///   todo 转移到实现类下
        /// </summary>
        public OwnerType owner_type { get; set; }

        /// <summary>
        ///  回退处理
        /// </summary>
        public RevertEffect revert_effect { get; set; }

        /// <summary>
        ///  失败处理
        /// </summary>
        public FailedEffect failed_effect { get; set; }

        /// <summary>
        ///  直接循环次数
        /// </summary>
        public int loop_times { get; set; } = 1;
    }



    public enum RevertEffect
    {
        /// <summary>
        ///  不需要处理
        /// </summary>
        None = 1,
        /// <summary>
        ///  失败后执行回退操作
        /// </summary>
        RevertSelf = 2,
        /// <summary>
        ///  除了回退自身同时回退所有所在群组其他可回退任务
        /// </summary>
        RevertAll = 4,
    }

    public enum FailedEffect
    {
        FailedSelf = 2,
        FailedAll = 4,
    }
}
