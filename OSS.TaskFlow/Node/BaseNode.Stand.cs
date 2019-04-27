using OSS.Common.ComModels;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  基础工作节点
    /// </summary>
    public abstract partial class BaseStandNode<TReq,TRes> :BaseNode<TReq,TRes> where TRes : ResultMo, new()
    {
   
    }
}
