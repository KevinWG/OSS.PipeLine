using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.TaskFlow.Flow.MetaMos;

namespace OSS.TaskFlow.Flow.Mos
{
    public class FlowContext
    {
        /// <summary>
        ///  当前系统运行Id
        /// </summary>
        public string run_id { get; set; }

        /// <summary>
        ///  当前流元信息
        /// </summary>
        public FlowMeta flow_meta { get; set; }
    }



}
