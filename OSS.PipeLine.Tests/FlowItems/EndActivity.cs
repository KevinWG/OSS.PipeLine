using System.Threading.Tasks;
using OSS.Tools.Log;

namespace OSS.Pipeline.Tests.FlowItems
{
    /// <summary>
    /// 空活动
    /// </summary>
    public class EmptyActivity : BaseActivity
    {
        /// <summary>
        ///  执行空操作
        /// </summary>
        /// <returns></returns>
        protected override Task<TrafficSignal> Executing()
        {
            LogHelper.Info("申请流程结束！");
            return Task.FromResult(TrafficSignal.Green_Pass);
        }
    }
}
