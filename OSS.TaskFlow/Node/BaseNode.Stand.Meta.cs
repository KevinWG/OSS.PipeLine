using OSS.TaskFlow.FlowLine.Mos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseStandNode<TReq>
    {
        public BaseStandNode()
        {
            InstanceType = InstanceType.Stand;
        }
    }

}
