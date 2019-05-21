using OSS.Common.ComModels;
using OSS.EventFlow.MetaMos;

namespace OSS.EventFlow
{
    /// <summary>
    /// 流运行时元数据信息
    /// </summary>
    public abstract partial class BaseFlow:BaseMetaProvider<FlowMeta>
    {
        private const string _moduleName = "OSS.EventFlow";
    
        /// <summary>
        ///  流节点信息
        /// </summary>
        public FlowMeta FlowMeta => GetConfig();

    }
}
