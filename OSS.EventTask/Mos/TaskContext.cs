using OSS.Common.ComModels;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Util;

namespace OSS.EventTask.Mos
{
    public class TaskContext<TDomain, TReq, TRes> : TaskContext<TRes>
        where TRes : ResultMo, new()
    {
        /// <summary>
        ///  请求信息
        /// </summary>
        public ExcuteReq<TDomain,TReq> req { get; set; }
    }

    /// <summary>
    ///   请求数据
    /// </summary>
    /// <typeparam name="TReq"></typeparam>
    /// <typeparam name="TRes"></typeparam>
    public class TaskContext<TReq, TRes> : TaskContext<TRes>
        where TRes : ResultMo, new()
    {
        public ExcuteReq<TReq> req { get; set; }
    }
    
    /// <summary>
    ///  任务上下文基类
    /// </summary>
    public class TaskContext<TRes>//:BaseTaskContext
     where TRes:ResultMo,new ()
    {
        #region 基础配置信息

        /// <summary>
        ///  task元信息【配置信息】
        /// </summary>
        public TaskMeta task_meta { get; set; }

        #endregion
        
        public RunCondition task_condition { get; set; }

        /// <summary>
        ///  运行状态
        /// </summary>
        public TaskRunStatus run_status { get;internal set; }

        /// <summary>
        ///  返回信息
        /// </summary>
        public TRes resp { get; set; }
    }
}
