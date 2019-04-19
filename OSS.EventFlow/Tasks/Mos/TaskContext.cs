namespace OSS.EventFlow.Tasks.Mos
{
    /// <summary>
    ///   上下文信息
    /// </summary>
    /// <typeparam name="TReq"></typeparam>
    public class TaskContext<TReq> : TaskBaseContext
    {
        public TaskContext()
        {
        }

        public TaskContext(TReq req)
        {
            Body = req;
        }

        /// <summary>
        ///   执行任务内容主体
        /// </summary>
        public TReq Body { get; set; }
    }

    /// <summary>
    ///  任务上下文基类
    /// </summary>
    public abstract class TaskBaseContext
    {
        /// <summary>
        ///  当前流Id
        /// </summary>
        public string ContextId { get; set; }

        /// <summary>
        ///   任务编码
        /// </summary>
        public string TaskCode { get; set; }

        /// <summary>
        ///   节点编码
        /// </summary>
        public string WorkerCode { get; set; }


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
