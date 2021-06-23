using OSS.DataFlow;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道流可选项
    /// </summary>
    public class PipeLineOption
    {
        /// <summary>
        ///  监控器
        /// </summary>
        public IPipeWatcher Watcher { get; set; }

        /// <summary>
        ///  监控器使用的消息流
        /// </summary>
        public string WatcherDataFlowKey { get; set; }

        /// <summary>
        ///  监控器消息流的可选项
        /// </summary>
        public DataFlowOption WatcherDataFlowOption { get; set; }
    }
}