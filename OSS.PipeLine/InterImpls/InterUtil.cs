using System.Threading.Tasks;
using OSS.Pipeline.Base.Mos;

namespace OSS.Pipeline
{
    /// <summary>
    ///  内部辅助类
    /// </summary>
    public static class InterUtil
    {
        /// <summary>
        ///  Task 的True值
        /// </summary>
        public static Task<bool> TrueTask { get; } = Task.FromResult(true);

        /// <summary>
        ///  Task的False值
        /// </summary>
        public static Task<bool> FalseTask { get; } = Task.FromResult(false);
        
        public static Task<TrafficSignal> Green { get; } = Task.FromResult(TrafficSignal.Green_Pass);
        public static Task<TrafficSignal> Yellow { get; } = Task.FromResult(TrafficSignal.Yellow_Wait);
        public static Task<TrafficSignal> Red { get; } = Task.FromResult(TrafficSignal.Red_Block);
    }
}
