using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.TaskFlow.Flow.Mos;
using OSS.TaskFlow.Tasks.Interfaces;

namespace OSS.TaskFlow.Flow
{
    public abstract partial class BaseFlow<TDomain> where TDomain: IDomainMo
    {
        public async Task<ResultMo> Enter(FlowReq req)
        {

            //  todo 获取领域信息
            //  todo 赋值领域Id => run_id

            var context = new FlowContext();

            var checkRes =  context.CheckFlowContext();
            if (!checkRes.IsSuccess())
                return checkRes;

            return new ResultMo();
        }

        public abstract Task End();
    }

}
