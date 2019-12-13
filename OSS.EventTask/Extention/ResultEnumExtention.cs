using OSS.Common.Extention;

namespace OSS.EventTask.Extention
{
    public enum TaskRunStatus
    {
        /// <summary>
        ///  等待运行
        /// </summary>
        [OSDescript("等待运行")] WaitToRun = 0,

        /// <summary>
        ///  运行暂停
        /// </summary>
        [OSDescript("运行暂停")] RunPaused = 10,

        /// <summary>
        /// 运行失败
        /// </summary>
        [OSDescript("运行失败")] RunFailed = 20,

        /// <summary>
        /// 运行成功
        /// </summary>
        [OSDescript("运行成功")] RunCompoleted = 30,

        /// <summary>
        /// 回退
        /// </summary>
        [OSDescript("回退")] RunReverted = 50

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
            return res == TaskRunStatus.RunCompoleted;
        }

        public static bool IsPaused(this TaskRunStatus res)
        {
            return res == TaskRunStatus.RunPaused;
        }

        public static bool IsReverted(this TaskRunStatus res)
        {
            return res == TaskRunStatus.RunReverted;
        }

        public static bool IsWaitToRun(this TaskRunStatus res)
        {
            return res == TaskRunStatus.WaitToRun;
        }
        
    }
}
