using OSS.EventTask.Extension;

namespace OSS.EventTask.Mos
{
    public abstract class BaseTaskResp
    {
        /// <summary>
        ///  运行状态
        /// </summary>
        public TaskRunStatus run_status { get; internal set; }

        /// <summary>
        ///   回退处理过
        /// </summary>
        public bool has_reverted { get; set; }

        /// <summary>
        ///  间隔执行次数
        /// </summary>
        public int tried_times { get; internal set; }

        /// <summary>
        ///  当前执行时间戳
        /// </summary>
        public long executed_time { get; set; }

        /// <summary>
        ///  下次时间戳
        /// </summary>
        public long next_time { get; set; }
    }

    public abstract class BaseTaskResp<TRes> : BaseTaskResp
    {
        /// <summary>
        ///  返回信息
        /// </summary>
        public TRes resp { get; internal set; }
    }

    public class TaskResp<TRes> : BaseTaskResp<TRes>
    {

        /// <summary>
        ///  单词执行内部循环错误
        /// </summary>
        public int loop_times { get; set; } = 1;
    }
    
    public class DoResp< TRes>
    {
        /// <summary>
        ///  运行状态
        ///     = TaskRunStatus.RunFailed 系统会字段判断是否满足重试条件执行重试
        /// </summary>
        public TaskRunStatus run_status { get; set; }

        /// <summary>
        ///  返回信息
        /// </summary>
        public TRes resp { get; set; }
    }

    public static class TaskResponseExtention
    {
        

        public static void SetToTaskResp<TRes>(this DoResp<TRes> res, TaskResp<TRes> taskResp)
               where TRes :new()
        {
            taskResp.run_status = res.run_status;
            taskResp.resp = res.resp;
        }
    }


}
