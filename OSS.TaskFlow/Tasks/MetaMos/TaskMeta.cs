using System;
using System.Collections.Generic;
using System.Text;

namespace OSS.TaskFlow.Tasks.MetaMos
{
    public class TaskMeta
    {
        /// <summary>
        ///  任务键
        /// </summary>
        public string task_key { get; set; }

        /// <summary>
        ///  任务名称
        /// </summary>
        public string task_name { get; set; }


        public int status { get; set; }

    }


    public enum TaskStatus
    {
        Enable
    }

}
