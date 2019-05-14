using System.Threading.Tasks;
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
    public abstract partial class BaseDomainTask<TDomain, TReq, TRes> 
    {
        /// <summary>
        ///  执行任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<TaskResponse<TRes>> Run(ExecuteData<TDomain,TReq> req)
        {
            return Run(req,new RunCondition());
        }

     
        #region 内部方法扩展

        internal override TRes RunCheckInternal(ExecuteData<TDomain, TReq> req, RunCondition runCondition)
        {
            if (req.req_data == null)
                return new TRes().WithResult(SysResultTypes.ApplicationError, "Task must Run with request info!");

            if (req.domain_data == null)
                return new TRes().WithResult(SysResultTypes.ApplicationError, "Domain task must Run with domain_data!");

            return base.RunCheckInternal(req,runCondition);
        }

        #endregion
    }
}