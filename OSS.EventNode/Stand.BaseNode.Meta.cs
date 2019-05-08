using OSS.EventNode.Interfaces;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{
    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseStandNode<TReq, TRes>
    {
        protected BaseStandNode()
        {
            InstanceNodeType = InstanceType.Stand;
        }
        
        #region 存储处理

        public IStandNodeProvider<TReq> MetaProvider => (IStandNodeProvider<TReq>)m_metaProvider;

        public void RegisteProvider(IStandNodeProvider<TReq> metaPro)
        {
            base.RegisteProvider_Internal(metaPro);
        }
        
        #endregion

    }

}
