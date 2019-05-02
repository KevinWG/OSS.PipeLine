using OSS.EventTask.Interfaces;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{
    public abstract partial class BaseStandTask<TReq, TRes> 
    {
        protected BaseStandTask()
        {
            InstanceType = InstanceType.Stand;
        }

        #region 存储处理

        public IStandTaskProvider<TReq> MetaProvider => (IStandTaskProvider<TReq>)m_metaProvider;
        
        public void RegisteProvider(IStandTaskProvider<TReq> metaPro)
        {
            base.RegisteProvider_Internal(metaPro);
        }


        #endregion
       
    }
}