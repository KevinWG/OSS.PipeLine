using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;

namespace OSS.EventTask
{
    /// <summary>
    /// 基础领域任务
    /// </summary>
    /// <typeparam name="TReq"></typeparam>
    /// <typeparam name="TDomain"></typeparam>
    /// <typeparam name="TRes"></typeparam>
    public abstract partial class BaseDomainTask<TDomain, TReq, TRes> : BaseTask<TaskContext<TDomain,TReq,TRes>, TRes>
        where TRes : ResultMo, new()
    {
        internal override TRes RunCheck(TaskContext<TDomain, TReq, TRes> context, RunCondition runCondition)
        {
            if (context.req == null)
                return new TRes().SetErrorResult(SysResultTypes.ApplicationError, "Task must Run with request info!");
               
            if (context.req.domain_data == null)
                return new TRes().SetErrorResult(SysResultTypes.ApplicationError, "Domain task must Run with domain_data!");
       
            return base.RunCheck(context, runCondition);
        }
    }
}