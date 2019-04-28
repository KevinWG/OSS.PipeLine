using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;

namespace OSS.TaskFlow.Tasks.Mos
{
    public static class EventFlowResultExtention
    {
        /// <summary>
        ///  是否任务执行失败
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public static bool IsTaskFailed(this ResultMo res)
        {
            return res.sys_ret == (int) SysResultTypes.RunFailed;
        }

        /// <summary>
        /// 是否等待任务重试
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public static bool IsTaskPause(this ResultMo res)
        {
            return res.sys_ret == (int)SysResultTypes.RunPause;
        }


    }
}
