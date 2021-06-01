using System.Collections.Generic;
using OSS.Pipeline.Gateway;
using OSS.Tools.Log;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class PayGateway : BaseBranchGateway<PayContext>
    {
        public PayGateway()
        {
            PipeCode = "PayGateway";
        }
        protected override IEnumerable<BasePipePart<PayContext>> FilterNextPipes(List<BasePipePart<PayContext>> branchItems, PayContext context)
        {
            LogHelper.Info("这里进行支付通过后的分流");
            return branchItems;
        }
    }
}
