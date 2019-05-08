using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{
    public abstract partial class BaseStandTask<TReq, TRes> 
    {
        /// <summary>
        ///  执行任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<TaskResponse<TRes>> Run(ExcuteReq<TReq> req)
        {
            return Run(req, new RunCondition());
        }


        #region 内部方法扩展

        internal override TRes RunCheckInternal(ExcuteReq<TReq> req, RunCondition runCondition)
        {
            if (req.req_data == null)
                return new TRes().WithResult(SysResultTypes.ApplicationError, "Task must Run with request info!");

           return base.RunCheckInternal(req, runCondition);
        }

        #endregion

    }
}