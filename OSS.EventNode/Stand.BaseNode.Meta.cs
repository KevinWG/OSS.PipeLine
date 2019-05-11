using OSS.Common.ComModels;
using OSS.EventNode.Interfaces;
using OSS.EventNode.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{
    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseStandNode<TReq, TRes> : BaseNode<ExcuteReq<TReq>, TRes>
        where TRes : ResultMo, new()
    {
        #region 构造函数

        protected BaseStandNode() : this(null)
        {
        }

        protected BaseStandNode(NodeMeta node) : base(node)
        {
            InstanceNodeType = InstanceType.Stand;
        }

        #endregion

        #region 存储处理

        //public IStandNodeProvider<ExcuteReq<TReq>> MetaProvider => (IStandNodeProvider<ExcuteReq<TReq>>) m_metaProvider;

        //public void RegisteProvider(IStandNodeProvider<ExcuteReq<TReq>> metaPro)
        //{
        //    m_metaProvider = metaPro;
        //}

        #endregion
    }
}
