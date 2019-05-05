using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{
    /// <summary>
    /// 基础领域任务
    /// </summary>
    /// <typeparam name="TReq"></typeparam>
    /// <typeparam name="TDomain"></typeparam>
    /// <typeparam name="TRes"></typeparam>
    public abstract partial class BaseDomainTask<TReq, TDomain, TRes> : BaseTask<TaskContext<TReq, TDomain>, TRes>
        where TRes : ResultMo, new()
    {
        //public virtual async Task<TTRes> Process(me)
        //{
            
        //}
       internal override ResultMo ProcessCheck(TaskContext<TReq, TDomain> context,RunCondition runCondition)
        {
            if (context.domain_data == null)
            {
                return new ResultMo(SysResultTypes.InnerError, ResultTypes.ObjectNull,
                    "Domain task must process with domain_data!");
            }
            return base.ProcessCheck(context, runCondition);
        }
    }
}