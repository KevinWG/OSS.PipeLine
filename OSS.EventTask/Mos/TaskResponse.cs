using OSS.EventTask.Extension;
using OSS.EventTask.MetaMos;

namespace OSS.EventTask.Mos
{
    public abstract class BaseTaskResp<TMeta, TTRes>
    {       
        /// <summary>
        ///   元数据信息
        /// </summary>
        public TMeta meta { get; internal set; }

        /// <summary>
        ///  运行状态
        /// </summary>
        public TaskRunStatus run_status { get; internal set; }

        /// <summary>
        ///   当次执行是否已经回退
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


        /// <summary>
        ///  返回信息
        /// </summary>
        public TTRes resp { get; internal set; }
    }



    public class EventTaskResp<TTRes> : BaseTaskResp<EventTaskMeta,TTRes>
    {
        /// <summary>
        ///  单词执行内部循环错误
        /// </summary>
        public int loop_times { get; set; } = 1;
    }




    public class DoResp< TTRes>
    {
        /// <summary>
        ///  运行状态
        ///     = TaskRunStatus.RunFailed 系统会字段判断是否满足重试条件执行重试
        /// </summary>
        public TaskRunStatus run_status { get; set; }

        /// <summary>
        ///  返回信息
        /// </summary>
        public TTRes resp { get; set; }
    }

    public static class TaskResponseExtension
    {
        public static void SetToTaskResp<TRes>(this DoResp<TRes> res, EventTaskResp<TRes> taskResp)
               where TRes :new()
        {
            taskResp.run_status = res.run_status;
            taskResp.resp = res.resp;
        }


      
    }


}
