namespace OSS.EventFlow.Tasks.Mos
{
    public class TaskContext<TReq> : TaskContext
    {
        public TaskContext()
        {
        }

        public TaskContext(TReq req)
        {
            Body = req;
        }

        public TReq Body { get; set; }
    }

    public abstract class TaskContext
    {
        /// <summary>
        ///  上下文Id
        /// </summary>
        public string WorkId { get; set; }

        /// <summary>
        ///  当前执行任务编号
        /// </summary>
        public string TaskCode { get; set; }

        /// <summary>
        ///  当前执行任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        ///  执行总次数
        /// </summary>
        public int ExcutedTimes { get;internal set; }
        
        /// <summary>
        ///  间隔执行次数
        /// </summary>
        public int IntervalTimes { get; internal set; }

        /// <summary>
        ///  上次执行时间
        /// </summary>
        public int LastExcuteDate { get; set; }

    }
}
