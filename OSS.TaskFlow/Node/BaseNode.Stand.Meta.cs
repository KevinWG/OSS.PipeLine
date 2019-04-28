using OSS.TaskFlow.Node.MetaMos;

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
    }

}
