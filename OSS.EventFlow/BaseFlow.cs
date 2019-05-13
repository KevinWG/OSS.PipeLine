using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow
{
    public abstract partial class BaseFlow<TDomain>
    {
        public async Task<ResultMo> Enter(FlowReq req)
        {
            //  todo 获取领域信息
            var context = new FlowContext();

            CheckInitailContext(context);

            return new ResultMo();
        }


        private static void CheckInitailContext(FlowContext context)
        {
            if (string.IsNullOrEmpty(context.flow_meta?.flow_id))
            {
                throw new ResultException(SysResultTypes.ApplicationError, "flow metainfo has error!");
            }
         
        }

        public abstract Task End();
    }

}
