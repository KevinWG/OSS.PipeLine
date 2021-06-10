using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <summary>
    ///  内部辅助类
    /// </summary>
    internal static class InterUtil
    {
        /// <summary>
        ///  Task 的True值
        /// </summary>
        public static Task<bool> TrueTask { get; } = Task.FromResult(true);
    }
}
