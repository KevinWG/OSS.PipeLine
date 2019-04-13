
namespace OSS.EventFlow.NodeWorker
{
    /// <summary>
    ///  基础工作者（内部可重试回退等操作）
    /// </summary>
    public abstract class BaseWorker
    {
        public string WorderId { get; set; }
    }
}
