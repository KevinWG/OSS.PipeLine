
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using OSS.DataFlow;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.InterImpls.Watcher
{
    internal class PipeWatcherProxy
    {
        private readonly IPipeWatcher                  _watcher;
        private readonly IDataPublisher<WatchDataItem> _publisher;
        private readonly ActionBlock<WatchDataItem>    _watchDataQueue;

        public PipeWatcherProxy(IPipeWatcher watcher, string dataFlowKey, DataFlowOption option)
        {
            if (!string.IsNullOrEmpty(dataFlowKey))
            {
                _publisher = DataFlowFactory.CreateFlow<WatchDataItem>(dataFlowKey, WatchCallBack, option);
            }
            else
            {
                _watchDataQueue = new ActionBlock<WatchDataItem>(WatchCallBack,
                    new ExecutionDataflowBlockOptions()
                    {
                        MaxDegreeOfParallelism = 4
                    });
            }
            _watcher = watcher;
        }

        async Task<bool> WatchCallBack(WatchDataItem data)
        {
            try
            {
                // await 保证如果出现异常能在当前线程拦截
                //  避免造成触发队列  Complete 
                switch (data.ActionType)
                {
                    case WatchActionType.Starting:
                        return await _watcher.Starting(data.PipeCode, data.PipeType, data.Para);
                    case WatchActionType.Executed:
                        return await _watcher.Excuted(data.PipeCode, data.PipeType, data.Para, data.Result);
                    case WatchActionType.Blocked:
                        return await _watcher.Blocked(data.PipeCode, data.PipeType, data.Para);
                }
            }
            catch 
            {
            }
            return false;
        }


        public Task Watch(WatchDataItem data)
        {
            if (_publisher != null)
            {
                return _publisher.Publish(data);
            }

            _watchDataQueue.Post(data);
            return Task.CompletedTask;
        }

    }

    internal struct WatchDataItem
    {
        public string PipeCode { get; set; }

        public PipeType PipeType { get; set; }

        public WatchActionType ActionType { get; set; }

        public object Para { get; set; }

        public TrafficResult Result { get; set; }
    }


    internal enum WatchActionType
    {
        /// <summary>
        ///  开始
        /// </summary>
        Starting,

        /// <summary>
        ///  执行完成
        /// </summary>
        Executed,

        /// <summary>
        ///  堵塞
        /// </summary>
        Blocked,
    }

}
