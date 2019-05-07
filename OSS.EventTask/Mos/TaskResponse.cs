using OSS.EventTask.Util;

namespace OSS.EventTask.Mos
{
    public class TaskResponse<TRes>
    {
        /// <summary>
        ///  运行状态
        /// </summary>
        public TaskRunStatus run_status { get; internal set; }

        public RunCondition task_condition { get; set; }

        /// <summary>
        ///  返回信息
        /// </summary>
        public TRes resp { get; internal set; }
    }


}
