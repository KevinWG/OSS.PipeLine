
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <summary>
    ///  空组件(多用于开始结尾)
    /// </summary>
    public class EmptyActivity : BaseActivity
    {
        public EmptyActivity(string pipeCode = null) : base(pipeCode)
        {
        }
        private static readonly Task<TrafficSignal> _result = Task.FromResult(TrafficSignal.GreenSignal);
        protected override Task<TrafficSignal> Executing()
        {
            return _result;
        }
    }

    /// <summary>
    ///  空组件(多用于开始结尾)
    /// </summary>
    public class EmptyActivity<TContext> : BaseActivity<TContext>
    {
        /// <summary>
        ///  空组件
        /// </summary>
        /// <param name="pipeCode"></param>
        public EmptyActivity(string pipeCode = null) : base(pipeCode)
        {
        }

        private static readonly Task<TrafficSignal> _result = Task.FromResult(TrafficSignal.GreenSignal);
        protected override Task<TrafficSignal> Executing(TContext para)
        {
            return _result;
        }
    }
}
