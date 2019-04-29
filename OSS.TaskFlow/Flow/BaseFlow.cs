using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.TaskFlow.Flow.Mos;
using OSS.TaskFlow.Tasks.Interfaces;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Flow
{
    public abstract partial class BaseFlow<TDomain> where TDomain: IDomainMo
    {
        public async Task<ResultMo> Enter(FlowReq req)
        {
            var context = new FlowContext();

            var checkRes =
                await context.CheckFlowContext(InstanceType.Domain, () => MetaProvider.GenerateRunId(context));
            if (!checkRes.IsSuccess())
                return checkRes;

            return new ResultMo();
        }

        public abstract Task End();
    }

}
