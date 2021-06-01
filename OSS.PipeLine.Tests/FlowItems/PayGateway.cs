﻿using System.Collections.Generic;
using OSS.PipeLine.Gateway;
using OSS.Tools.Log;

namespace OSS.PipeLine.Tests.FlowItems
{
    public class PayGateway : BaseBranchGateway<PayContext>
    {
        public PayGateway()
        {
            PipeCode = "PayGateway";
        }
        protected override IEnumerable<BasePipe<PayContext>> FilterNextPipes(List<BasePipe<PayContext>> branchItems, PayContext context)
        {
            LogHelper.Info("这里进行支付通过后的分流");
            return branchItems;
        }
    }
}