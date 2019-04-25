using OSS.TaskFlow.Node.MetaMos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  基础工作者
    /// </summary>
    public abstract partial class BaseNode<TReq>
    {
        /// <summary>
        ///  节点信息
        /// </summary>
        public NodeMeta NodeMeta { get; set; }
        
    }
}
