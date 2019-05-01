using OSS.EventTask.Mos;
using OSS.TaskFlow.Node.Interfaces;

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

        public IFlowNodeProvider<TReq, TDomain> MetaProvider => (IFlowNodeProvider<TReq, TDomain>) m_metaProvider;

        public void RegisteProvider(IFlowNodeProvider<TReq, TDomain> metaPro)
        {
            base.RegisteProvider_Internal(metaPro);
        }
        
        #endregion
    }
}
