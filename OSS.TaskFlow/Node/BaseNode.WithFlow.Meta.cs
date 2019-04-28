using OSS.TaskFlow.Node.MetaMos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseFlowNode<TReq, TFlowData, TRes>
    {
        protected BaseFlowNode()
        {
            InstanceType = InstanceType.WithFlow;
        }

    }

}
