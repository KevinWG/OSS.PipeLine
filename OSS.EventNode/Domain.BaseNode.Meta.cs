using OSS.EventNode.Interfaces;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{
    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseDomainNode<TReq, TDomain, TRes>
    {
        protected BaseDomainNode()
        {
           InstanceType = InstanceType.Domain;
        }

        #region 存储处理

        public IDomainNodeProvider<TReq, TDomain> MetaProvider => (IDomainNodeProvider<TReq, TDomain>) m_metaProvider;

        public void RegisteProvider(IDomainNodeProvider<TReq, TDomain> metaPro)
        {
            base.RegisteProvider_Internal(metaPro);
        }
        
        #endregion
    }
}
