using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.TaskFlow.Flow.Mos;

namespace OSS.TaskFlow.Flow
{
    public abstract partial class BaseFlow<TDomain> 
    {
        public async Task<ResultMo> Enter(FlowReq req)
        {

            //  todo 获取领域信息
            //  todo 赋值领域Id => run_id

            var context = new FlowContext();

            CheckInitailContext(context);

 
            return new ResultMo();
        }



        private static void CheckInitailContext(FlowContext context)
        {
            if (string.IsNullOrEmpty(context.flow_meta?.flow_key))
            {
                throw new ResultException(SysResultTypes.ConfigError, ResultTypes.InnerError, "flow metainfo has error!");
            }
            if (string.IsNullOrEmpty(context.run_id))
                context.run_id = DateTime.Now.Ticks.ToString();
            
        }

        public abstract Task End();
    }

}
