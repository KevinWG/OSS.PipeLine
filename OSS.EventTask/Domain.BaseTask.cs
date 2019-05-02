using OSS.Common.ComModels;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{
    /// <summary>
    /// 基础领域任务
    ///     todo 获取领域信息
    /// </summary>
    /// <typeparam name="TReq"></typeparam>
    /// <typeparam name="TDomain"></typeparam>
    /// <typeparam name="TRes"></typeparam>
    public abstract partial class BaseDomainTask<TReq, TDomain, TRes> : BaseTask<TaskContext<TReq, TDomain>, TRes>
        where TRes : ResultMo, new()
    {
       
    }
}