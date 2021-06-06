using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <summary>
    ///  内部辅助类
    /// </summary>
    public static class InterUtil
    {
        /// <summary>
        ///  true 的task true类型
        /// </summary>
        public static Task<bool> TrueTask { get; } = Task.FromResult(true);
    }
}
