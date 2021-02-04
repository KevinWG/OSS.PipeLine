using System.Collections.Generic;
using OSS.EventFlow;
using OSS.EventFlow.Gateway;
using OSS.Tools.Log;

namespace OSS.TaskFlow.Tests.FlowItems
{
    public class PayGateway:BaseBranchGateway<PayContext>
    {
        protected override IEnumerable<BasePipe<PayContext>> FilterNextPipes(List<BasePipe<PayContext>> branchItems, PayContext context)
        {
            LogHelper.Info("这里进行支付通过后的分流");
            return branchItems;
        }
    }
}
