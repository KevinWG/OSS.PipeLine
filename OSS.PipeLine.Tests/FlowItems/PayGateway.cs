using System.Collections.Generic;
using OSS.Pipeline.InterImpls.GateWay;
using OSS.Tools.Log;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class PayGateway : BaseBranchGateway<PayContext>
    {
        public PayGateway()
        {
            PipeCode = "PayGateway";
        }

        protected override IEnumerable<IBranchNodePipe> SelectNextPipes( PayContext context, List<IBranchNodePipe> branchItems)
        {
            LogHelper.Info("这里进行支付通过后的分流");
            return branchItems;
        }
    }
}
