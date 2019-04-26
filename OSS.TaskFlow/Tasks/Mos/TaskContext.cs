using OSS.TaskFlow.Tasks.MetaMos;

namespace OSS.TaskFlow.Tasks.Mos
{
    /// <summary>
    ///   上下文信息
    /// </summary>
    /// <typeparam name="TReq"></typeparam>
    public class TaskContext<TReq> : TaskBaseContext
    {
        public TaskContext(){}

        public TaskContext(TReq req)
        {
            body = req;
        }

        /// <summary>
        ///   执行任务内容主体
        /// </summary>
        public TReq body { get; set; }
        
        public object flow_data { get; set; }
    }

    /// <summary>
    ///  任务上下文基类
    /// </summary>
    public abstract class TaskBaseContext
    {
        #region 元数据信息

        public TaskMeta  task_meta { get; set; }

        #endregion
        
        /// <summary>
        ///  实例流Id
        /// </summary>
        public string flow_id { get; set; }

        /// <summary>
        ///  执行总次数
        /// </summary>
        public int exced_times { get;internal set; }
        
        /// <summary>
        ///  间隔执行次数
        /// </summary>
        public int interval_times { get; internal set; }

        /// <summary>
        ///  上次执行时间
        /// </summary>
        public long last_excutedate { get; set; }

    }
}
