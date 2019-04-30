using OSS.TaskFlow.Node.Interfaces;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseStandNode<TReq, TRes>
    {
        protected BaseStandNode()
        {
            InstanceType = InstanceType.Stand;
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
