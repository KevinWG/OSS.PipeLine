namespace OSS.EventTask.Mos
{
    public class RunCondition
    {
        /// <summary>
        ///  单词执行内部循环错误
        /// </summary>
        public int loop_times { get; set; }

        /// <summary>
        ///  间隔执行次数
        /// </summary>
        public int tried_times { get; internal set; }

        /// <summary>
        ///  当前执行时间戳
        /// </summary>
        public long run_timestamp { get; set; }

        /// <summary>
        ///  下次时间戳
        /// </summary>
        public long next_timestamp { get; set; }
    }
}
