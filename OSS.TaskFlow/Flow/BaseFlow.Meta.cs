using OSS.TaskFlow.Flow.Interfaces;
using OSS.TaskFlow.Flow.MetaMos;

namespace OSS.TaskFlow.Flow
{
    /// <summary>
    /// 流运行时元数据信息
    /// </summary>
    public abstract partial class BaseFlow<TFlowData>
    {
        // todo 配置处理
        // todo  1。 读取配置初始化信息
        //       2. 每个节点对应的核心数据状态的变化信息（特殊定制化才有）
        
        /// <summary>
        ///   当前流编码
        /// </summary>
        public FlowMeta  FlowMeta { get; set; }
        
        #region 存储处理

        public IFlowProvider<TFlowData> MetaProvider { get; private set; }

        public void RegisteProvider(IFlowProvider<TFlowData> metaPro)
        {
            MetaProvider = metaPro;
        }

        #endregion


    }
}
