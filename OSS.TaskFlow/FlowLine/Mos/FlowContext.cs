using OSS.TaskFlow.FlowLine.MetaMos;

namespace OSS.TaskFlow.FlowLine.Mos
{
    public class FlowData<TFlowData>
    {
        /// <summary>
        ///  核心流运行数据
        /// </summary>
        public TFlowData flow_data { get; set; }
    }

    public class FlowContext
    {
        /// <summary>
        ///  当前系统运行Id
        /// </summary>
        public string run_id { get; set; }

        /// <summary>
        ///  当前流元信息
        /// </summary>
        public FlowMeta FlowMeta { get; set; }
    }
}
