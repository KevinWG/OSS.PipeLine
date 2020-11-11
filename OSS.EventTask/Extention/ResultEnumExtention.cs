using System;

namespace OSS.EventTask.Extention
{
    public enum TaskRunStatus
    {
        /// <summary>
        /// 运行失败
        /// </summary>
        RunFailed = -30,

        /// <summary>
        ///  等待运行
        /// </summary>
        WaitToRun = 0,

        /// <summary>
        ///  中断等待
        /// </summary>
        RunPaused = 10,
        
        /// <summary>
        /// 运行成功
        /// </summary>
        RunCompleted = 30
    }

    public static class TaskRunStatusExtention
    {
        /// <summary>
        ///  是否任务执行失败
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public static bool IsFailed(this TaskRunStatus res)
        {
            return res == TaskRunStatus.RunFailed;
        }

        public static bool IsCompleted(this TaskRunStatus res)
        {
            return res == TaskRunStatus.RunCompleted;
        }

        public static bool IsPaused(this TaskRunStatus res)
        {
            return res == TaskRunStatus.RunPaused;
        }

        //public static bool IsReverted(this TaskRunStatus res)
        //{
        //    return res == TaskRunStatus.RunReverted;
        //}

        public static bool IsWaitToRun(this TaskRunStatus res)
        {
            return res == TaskRunStatus.WaitToRun;
        }


        private static readonly long startTicks = new DateTime(1970, 1, 1).Ticks;


        /// <summary>
        /// 获取距离 1970-01-01（格林威治时间）的秒数
        /// </summary>
        /// <param name="localTime"></param>
        /// <returns></returns>
        public static long ToUtcSeconds(this DateTime localTime)
        {
            return (localTime.ToUniversalTime().Ticks - startTicks) / 10000000;
        }

    }
}
