namespace OSS.EventTask.Mos
{
    public class RunCondition
    {
        /// <summary>
        ///  执行总次数
        /// </summary>
        public int exced_times { get; internal set; }

        /// <summary>
        ///  间隔执行次数
        /// </summary>
        public int interval_times { get; internal set; }

        /// <summary>
        ///  上次执行时间
        /// </summary>
        public long last_Processdate { get; set; }
    }
}
