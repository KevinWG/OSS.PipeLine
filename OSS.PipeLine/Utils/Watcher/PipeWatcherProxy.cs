#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  管道监视器代理
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion


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
        private readonly string                        _dataFlowKey;

        public PipeWatcherProxy(IPipeWatcher watcher, string dataFlowKey, DataFlowOption option)
        {
            if (!string.IsNullOrEmpty(dataFlowKey))
            {
                _dataFlowKey = dataFlowKey;
                _publisher   = DataFlowFactory.CreateFlow<WatchDataItem>(dataFlowKey, WatchCallBack, option);
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
                    case WatchActionType.PreCall:
                        return await _watcher.PreCall(data.PipeCode, data.PipeType, data.Para).ConfigureAwait(false);
                    case WatchActionType.Executed:
                        return await _watcher.Executed(data.PipeCode, data.PipeType, data.Para, data.Result).ConfigureAwait(false);
                    case WatchActionType.Blocked:
                        return await _watcher.Blocked(data.PipeCode, data.PipeType, data.Para, data.Result).ConfigureAwait(false);
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
                return _publisher.Publish(_dataFlowKey,data);
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

        public WatchResult Result { get; set; }
    }


    internal enum WatchActionType
    {
        /// <summary>
        ///  上游管道调用
        /// </summary>
        PreCall,

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
