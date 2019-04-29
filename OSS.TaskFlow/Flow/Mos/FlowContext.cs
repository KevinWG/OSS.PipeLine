using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.TaskFlow.Flow.MetaMos;
using OSS.TaskFlow.Tasks.Mos;

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
        public static async Task<ResultMo> CheckFlowContext(this FlowContext context, InstanceType insType,
            Func<Task<ResultIdMo>> idGenerater=null)
        {
            if (string.IsNullOrEmpty(context.flow_meta?.flow_key))
            {
                return new ResultMo(SysResultTypes.ConfigError, ResultTypes.InnerError, "flow metainfo has error!");
            }

            if (!string.IsNullOrEmpty(context.run_id))
                return new ResultMo();
            
            if (idGenerater != null)
            {
                var idRes = await idGenerater.Invoke();
                if (!idRes.IsSuccess())
                    return idRes;

                context.run_id = idRes.id;
            }

            return string.IsNullOrEmpty(context.run_id) 
                ?  new ResultMo(SysResultTypes.InnerError, ResultTypes.InnerError, "run_id can't be null !") 
                :  new ResultMo();
        }
    }
}
