using OSS.TaskFlow.FlowLine.Mos;
using OSS.TaskFlow.Node.Mos;
using OSS.TaskFlow.Tasks.MetaMos;

namespace OSS.TaskFlow.Tasks.Mos
{
    /// <summary>
    ///  任务上下文基类
    /// </summary>
    public class TaskContext:NodeContext
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
