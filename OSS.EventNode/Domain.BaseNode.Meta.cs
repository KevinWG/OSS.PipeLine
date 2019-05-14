using OSS.Common.ComModels;
using OSS.EventNode.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{
    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseDomainNode<TDomain, TReq, TRes> : BaseNode<ExecuteData<TDomain, TReq>, TRes>
        where TRes : ResultMo, new()
    {
        #region 构造函数

        protected BaseDomainNode() : this(null)
        {
        }

        protected BaseDomainNode(NodeMeta node) : base(node)
        {
            InstanceNodeType = InstanceType.Stand;
        }

        #endregion

    }
}
