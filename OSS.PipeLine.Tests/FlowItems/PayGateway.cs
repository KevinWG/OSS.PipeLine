using System.Collections.Generic;
using OSS.Pipeline.Base;
using OSS.Tools.Log;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class PayGateway : BaseBranchGateway<PayContext>
    {
        public PayGateway()
        {
            PipeCode = "PayGateway";
        }

        protected override IEnumerable<BaseInPipePart<PayContext>> FilterNextPipes(List<BaseInPipePart<PayContext>> branchItems, PayContext context)
        {
            LogHelper.Info("这里进行支付通过后的分流");
            return branchItems;
        }
    }
}
