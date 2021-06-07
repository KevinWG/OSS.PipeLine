
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

        Task<bool> WatchCallBack(WatchDataItem data)
        {
            switch (data.ActionType)
            {
                case WatchActionType.Starting:
                    return _watcher.Starting(data.PipeCode, data.PipeType, data.Data);
                case WatchActionType.Executed:
                    return _watcher.Excuted(data.PipeCode, data.PipeType, data.Data, data.Result);
                case WatchActionType.Blocked:
                    return _watcher.Blocked(data.PipeCode, data.PipeType, data.Data);
            }

            return InterUtil.TrueTask;
        }


        public Task<bool> Watch(WatchDataItem data)
        {
            if (data.PipeType >= PipeType.MsgFlow)
            {
                return InterUtil.TrueTask;
            }

            if (_publisher != null)
            {
                return _publisher.Publish(data);
            }

            _watchDataQueue.Post(data);
            return InterUtil.TrueTask;
        }

    }

    internal struct WatchDataItem
    {
        public string PipeCode { get; set; }

        public PipeType PipeType { get; set; }

        public WatchActionType ActionType { get; set; }

        public object Data { get; set; }

        public object Result { get; set; }
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
