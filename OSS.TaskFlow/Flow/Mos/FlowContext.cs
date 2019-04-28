using System;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.TaskFlow.Flow.MetaMos;

namespace OSS.TaskFlow.Flow.Mos
{
    //public class FlowData<TFlowData>
    //{
    //    /// <summary>
    //    ///  核心流运行数据
    //    /// </summary>
    //    public TFlowData flow_data { get; set; }
    //}

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
        public static ResultMo CheckFlowContext(this FlowContext context,Func<string> idGenerate=null)
        {
            if (string.IsNullOrEmpty(context.flow_meta?.flow_key))
            {
                return new ResultMo(SysResultTypes.ConfigError, ResultTypes.InnerError, "flow metainfo has error!");
            }
       
            if (string.IsNullOrEmpty(context.run_id))
            {
                context.run_id = idGenerate?.Invoke()??DateTime.Now.ToUtcTicks().ToString();
            }
            return new ResultMo();
        }
    }
}
