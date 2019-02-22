namespace OSS.EventFlow.Tasks.Mos
{
    //public class TaskContext<TReq> : BaseTaskContext
    //{
    //    public TReq Body { get; set; }
    //}

    public class TaskContext
    {
        /// <summary>
        ///  上下文Id
        /// </summary>
        public string ContextId { get; set; }

        /// <summary>
        ///  任务编号
        /// </summary>
        public string TaskCode { get; set; }

        /// <summary>
        ///  任务名称
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
