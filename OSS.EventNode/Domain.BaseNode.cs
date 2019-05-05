using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.EventNode.Mos;
using OSS.EventTask;
using OSS.EventTask.Interfaces;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{
    /// <summary>
    ///  基础领域节点
    /// </summary>
    public abstract partial class BaseDomainNode<TReq, TDomain, TRes> : BaseNode<NodeContext<TReq, TDomain>, TRes>
        where TRes : ResultMo, new()
    {

        #region 扩展方法

        /// <summary>
        /// 获取领域数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task<ResultMo<TDomain>> GetDomainData(NodeContext<TReq> context)
        {
            return Task.FromResult(new ResultMo<TDomain>(SysResultTypes.NoResponse, ResultTypes.ObjectNull,
                "domain data can't be null in domain node!"));
        }


        #endregion


        #region 内部扩展方法重写

        internal override async Task<TRes> GetTaskItemResult(IBaseTask task, NodeContext<TReq, TDomain> con)
        {
            var taskContext = new TaskContext<TReq, TDomain>(); //  todo  完善
            if (task.InstanceType == InstanceType.Domain)
            {
                var domainTask = (BaseDomainTask<TReq, TDomain, TRes>)task;
                return await domainTask.Process(taskContext);
            }
            else
            {
                var standTask = (BaseStandTask<TReq, TRes>)task;
                return await standTask.Process(taskContext);
            }
        }


        internal override async Task<ResultMo> CheckInitailNodeContext(NodeContext<TReq, TDomain> context)
        {
            var res = await base.CheckInitailNodeContext(context); // todo  添加获取领域信息 扩展方法
            if (!res.IsSuccess())
                return res;

            if (context.domain_data == null)
            {
                var domainRes = await GetDomainData(context);
                if (domainRes.IsSuccess() && domainRes.data != null)
                {
                    context.domain_data = domainRes.data;
                    return domainRes;
                }
                return new ResultMo<TDomain>(SysResultTypes.NoResponse, ResultTypes.ObjectNull,
                    "domain data can't be null in domain node!");
            }
            return res;
        }

        #endregion

    }
}
