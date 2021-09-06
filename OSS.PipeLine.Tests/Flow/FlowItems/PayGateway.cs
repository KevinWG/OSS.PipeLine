using OSS.Pipeline.Interface;
using OSS.Tools.Log;

namespace OSS.Pipeline.Tests.FlowItems
{
    public class PayGateway : BaseBranchGateway<PayContext>
    {
        public PayGateway():base("PayGateway")
        {
        }

        protected override bool FilterBranchCondition(PayContext branchContext, IPipeMeta branch, string prePipeCode)
        {
            LogHelper.Info($"通过{PipeCode}  判断分支 {branch.PipeCode} 是否满足分流条件！");
            return base.FilterBranchCondition(branchContext, branch, prePipeCode);
        }

   
    }
}
