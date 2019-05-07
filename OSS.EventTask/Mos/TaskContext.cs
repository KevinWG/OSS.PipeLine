using OSS.Common.ComModels;
using OSS.EventTask.Util;

namespace OSS.EventTask.Mos
{
    /// <summary>
    ///  任务上下文基类
    /// </summary>
    public class TaskContext<TTReq, TRes> : TaskContext<TRes>
        where TRes : ResultMo, new()
    {
        /// <summary>
        ///  返回信息
        /// </summary>
        public TRes resp { get; internal set; }
    }

    public class TaskContext<TTReq> 
    {
        public TTReq req { get; set; }

        public RunCondition task_condition { get; set; }

        /// <summary>
        ///  运行状态
        /// </summary>
        public TaskRunStatus run_status { get; internal set; }
    }


}
