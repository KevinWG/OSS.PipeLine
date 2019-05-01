﻿using OSS.TaskFlow.Tasks.MetaMos;

namespace OSS.TaskFlow.Tasks.Mos
{
    /// <summary>
    ///  任务上下文基类
    /// </summary>
    public class TaskContext
    {
        /// <summary>
        ///  当前系统运行Id
        /// </summary>
        public string run_id { get; set; }

        /// <summary>
        ///  task元信息
        /// </summary>
        public TaskMeta  task_meta { get; set; }
        
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
