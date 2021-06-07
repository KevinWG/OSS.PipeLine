using System.Threading.Tasks;

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
    }
}
