using OSS.EventTask.MetaMos;

namespace OSS.EventTask.Mos
{
    public class TaskContext<TReq, TDomain> : TaskContext<TReq>
    {
        /// <summary>
        ///   核心流数据
        /// </summary>
        public TDomain domain_data { get; set; }
    }

    /// <summary>
    ///   请求数据
    /// </summary>
    /// <typeparam name="TReq"></typeparam>
    public class TaskContext<TReq> : TaskContext
    {
        /// <summary>
        ///   执行请求内容主体
        /// </summary>
        public TReq req_data { get; set; }
    }
    
    /// <summary>
    ///  任务上下文基类
    /// </summary>
    public class TaskContext: BaseContext
    {
        public string flow_key { get; set; }

        public string node_key { get; set; }

        #region 基础配置信息

        /// <summary>
        ///  task元信息【配置信息】
        /// </summary>
        public TaskMeta  task_meta { get; set; }

        #endregion
    }

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



    public class BaseContext
    {
        #region 运行信息

        public string exc_id { get; set; }

        #endregion
    }



    //public static class TaskContextExtention
    //{
    //    public static TaskContext ConvertToTaskContext(this NodeContext node)
    //    {
    //        var taskCon=new TaskContext();

    //        taskCon.exc_id = node.exc_id;
    //        taskCon.flow_meta = node.flow_meta;
    //        taskCon.node_meta = node.node_meta;

    //        return taskCon;
    //    }
    //}
}
