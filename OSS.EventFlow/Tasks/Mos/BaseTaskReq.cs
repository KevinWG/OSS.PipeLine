namespace OSS.EventFlow.Tasks.Mos
{
    public class TaskReq<TReq> : BaseTaskReq
    {
        public TReq Body { get; set; }
    }

    public class BaseTaskReq
    {
        /// <summary>
        ///  执行总次数
        /// </summary>
        public int ExcutedTimes { get;internal set; }


        /// <summary>
        ///  间隔执行次数
        /// </summary>
        public int IntervalTimes { get; internal set; }
    }
}
