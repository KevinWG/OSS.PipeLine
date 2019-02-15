using OSS.Common.ComModels;

namespace OSS.EventFlow.Tasks
{
    public enum EventFlowResult
    {
        Failed=-100
    }

    public static class EventFlowResultExtention
    {
        /// <summary>
        ///  是否任务执行失败
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public static bool IsTaskFailed(this ResultMo res)
        {
            return res.ret == (int) EventFlowResult.Failed;
        }
    }
}
