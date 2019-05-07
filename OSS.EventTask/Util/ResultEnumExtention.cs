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
        [OSDescript("等待运行")] WaitToRun = 0,

        /// <summary>
        ///  运行暂停
        /// </summary>
        [OSDescript("运行暂停")] RunPaused = 10,
        
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

        public static TRes SetErrorResult<TRes>(this TRes res,SysResultTypes sysRet,ResultTypes ret,string eMsg)
            where TRes : ResultMo
        {
            res.msg = eMsg;
            res.ret = (int)ret;
            res.sys_ret = (int) sysRet;
            return res;
        }


        //public static TRes CheckConvertToResult<TRes>(this ResultMo res)
        //    where TRes : ResultMo, new()
        //{
        //    if (res is TRes tres)
        //        return tres;

        //    if (!res.IsSuccess())
        //        return res.ConvertToResultInherit<TRes>();

        //    return new TRes()
        //    {
        //        sys_ret = (int) SysResultTypes.InnerError,
        //        ret = (int) ResultTypes.InnerError,
        //        msg = $"Return value error! Can't convert to {typeof(TRes)}"
        //    };
        //}

    }
}
