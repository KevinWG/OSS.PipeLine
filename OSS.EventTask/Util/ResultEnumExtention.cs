using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;

namespace OSS.EventTask.Util
{
    public enum TaskRunStatus
    {
        /// <summary>
        ///  等待运行
        /// </summary>
        [OSDescript("等待运行")] WaitRun = 0,

        /// <summary>
        ///  运行暂停
        /// </summary>
        [OSDescript("运行暂停")] RunPause = 10,
        
        /// <summary>
        /// 运行回退
        /// </summary>
        [OSDescript("运行回退")] RunReverted = 20,

        /// <summary>
        /// 运行失败
        /// </summary>
        [OSDescript("运行失败")] RunFailed = 30,
        
        /// <summary>
        /// 运行失败
        /// </summary>
        [OSDescript("运行成功")] RunCompoleted = 40
    }

    public class TaskResultMo : ResultMo
    {
        //public 
    }


   
    public static class FlowResultExtention
    {
        /// <summary>
        ///  是否任务执行失败
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public static bool IsRunFailed(this ResultMo res)
        {
            return res.sys_ret == (int) SysResultTypes.RunFailed;
        }

        /// <summary>
        /// 是否等待任务重试
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public static bool IsRunPause(this ResultMo res)
        {
            return res.sys_ret == (int) SysResultTypes.RunPause;
        }

        public static TRes CheckConvertToResult<TRes>(this ResultMo res)
            where TRes : ResultMo, new()
        {
            if (res is TRes tres)
                return tres;

            if (!res.IsSuccess())
                return res.ConvertToResultInherit<TRes>();

            return new TRes()
            {
                sys_ret = (int) SysResultTypes.InnerError,
                ret = (int) ResultTypes.InnerError,
                msg = $"Return value error! Can't convert to {typeof(TRes)}"
            };
        }

    }
}
