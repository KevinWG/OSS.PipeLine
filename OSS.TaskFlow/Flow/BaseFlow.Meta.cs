using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Flow.MetaMos;
using OSS.TaskFlow.Node.MetaMos;

namespace OSS.TaskFlow.Flow
{
    /// <summary>
    /// 流运行时元数据信息
    /// </summary>
    public abstract partial class BaseFlow<TFlowEntity>
    {
        // todo 配置处理
        // todo  1。 读取配置初始化信息
        //       2. 每个节点对应的核心数据状态的变化信息（特殊定制化才有）
        
        /// <summary>
        ///   当前流编码
        /// </summary>
        public FlowMeta  FlowMeta { get; set; }
        
        public abstract Task<ResultListMo<NodeMeta>> GetTaskMetas(ExcuteReq context);
    }
}
