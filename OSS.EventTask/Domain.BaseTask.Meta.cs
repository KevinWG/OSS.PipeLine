using System.Threading.Tasks;
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

        public IDomainTaskProvider<TReq, TDomain> MetaProvider => (IDomainTaskProvider<TReq, TDomain>)m_metaProvider;

        public void RegisteProvider(IDomainTaskProvider<TReq, TDomain> metaPro)
        {
            base.RegisteProvider_Internal(metaPro);
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