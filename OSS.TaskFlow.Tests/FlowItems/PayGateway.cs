using System.Collections.Generic;
using OSS.EventFlow;
using OSS.EventFlow.Gateway;
using OSS.EventFlow.Mos;
using OSS.Tools.Log;

namespace OSS.TaskFlow.Tests.FlowItems
{
    public class PayGateway:BaseBranchGateway<PayContext>
    {
        public PayGateway()
        {
                pipe_code = "PayGateway";
        }

        protected override IEnumerable<BasePipe<PayContext>> FilterNextPipes(List<BasePipe<PayContext>> branchItems, PayContext context)
        {
            LogHelper.Info("这里进行支付通过后的分流");
            return branchItems;
        }
    }
}
