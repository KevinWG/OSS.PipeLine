﻿using OSS.EventFlow.Dispatcher;

namespace OSS.EventFlow.Tasks.Mos
{
    public class TaskOption
    {
        /// <summary>
        ///  任务名称
        /// </summary>
        public string TaskName { get; set; }



        ///// <summary>
        /////  是否能够复用，如果为True 则底层使用单例模式
        ///// </summary>
        //public bool IsReusable { get; set; }
    }
}
