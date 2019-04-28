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


    public static class FlowContextExtention
    {
        public static ResultMo CheckFlowContext(this FlowContext context)
        {
            if (string.IsNullOrEmpty(context.flow_meta?.flow_key))
            {
                return new ResultMo(SysResultTypes.ConfigError, ResultTypes.InnerError, "flow metainfo has error!");
            }
            
            return string.IsNullOrEmpty(context.run_id) 
                ? new ResultMo(SysResultTypes.InnerError, ResultTypes.InnerError, "run_id can't be null !") 
                : new ResultMo();
        }
    }
}
