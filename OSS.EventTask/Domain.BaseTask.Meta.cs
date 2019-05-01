using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.EventTask.Interfaces;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{

    public abstract partial class BaseDomainTask<TReq, TDomain, TRes>
    {
        protected BaseDomainTask()
        {
            InstanceType = InstanceType.Domain;
        }

        #region 存储处理

        public IDomainTaskProvider<TReq, TDomain> MetaProvider => (IDomainTaskProvider<TReq, TDomain>) m_metaProvider;

        public void RegisteProvider(IDomainTaskProvider<TReq, TDomain> metaPro)
        {
            base.RegisteProvider_Internal(metaPro);
        }


        #endregion
        
        #region   获取外部数据扩展方法

        /// <summary>
        ///  获取领域数据方法【仅当领域数据为空时调用】
        /// </summary>
        /// <returns></returns>
        protected virtual Task<ResultMo<TDomain>> GetDomainData(TaskContext context, TReq req)
        {
            return Task.FromResult(new ResultMo<TDomain>(SysResultTypes.NoResponse, ResultTypes.ObjectNull,
                "can't get domain data!"));
        }

        #endregion

        #region 重写基类方法

        internal override Task SaveTaskContext_Internal(TaskContext context, TaskReqData data)
        {
            return MetaProvider.SaveTaskContext(context, (TaskReqData<TReq, TDomain>) data);
        }

        #endregion
    }
}