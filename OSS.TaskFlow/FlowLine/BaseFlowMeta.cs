using OSS.TaskFlow.FlowLine.MetaMos;

namespace OSS.TaskFlow.FlowLine
{
    public abstract partial class BaseFlow
    {
        // todo 配置处理
        // todo  1。 读取配置初始化信息
        //       2. 每个节点对应的核心数据状态的变化信息（特殊定制化才有）


        /// <summary>
        ///   当前流编码
        /// </summary>
        public FlowMeta  FlowMeta { get; set; }


    }
}
