using OSS.EventFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tasks.Mos
{
    public enum TaskResultType
    {
        /// <summary>
        ///  执行失败
        /// </summary>
        Failed = -9999,

        /// <summary>
        /// 等待重试
        /// </summary>
        WatingRetry = 1
    }

    public static class EventFlowResultExtention
    {
        /// <summary>
        ///  是否任务执行失败
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public static bool IsTaskFailed(this TaskResultMo res)
        {
            return res.task_ret == (int) TaskResultType.Failed;
        }

        /// <summary>
        /// 是否等待任务重试
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public static bool IsTaskWaiting(this TaskResultMo res)
        {
            return res.task_ret == (int)TaskResultType.WatingRetry;
        }


    }
}
