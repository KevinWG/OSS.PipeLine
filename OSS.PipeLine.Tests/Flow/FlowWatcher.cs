using System.Threading.Tasks;
using OSS.Pipeline.Interface;
using OSS.Tools.Log;

namespace OSS.Pipeline.Tests.Flow
{
    public class FlowWatcher:IPipeLineWatcher
    {
        public Task PreCall(string pipeCode, PipeType pipeType, object input)
        {
            LogHelper.Info($"进入 {pipeCode} 管道","PipePreCall","PipelineWatcher");
            return Task.CompletedTask;
        }

        public Task Executed(string pipeCode, PipeType pipeType, object input, WatchResult watchResult)
        {
            LogHelper.Info($"管道 {pipeCode} 执行结束，结束信号：{watchResult.signal}", "PipeExecuted", "PipelineWatcher");
            return Task.CompletedTask;
        }

        public Task Blocked(string pipeCode, PipeType pipeType, object input, WatchResult watchResult)
        {
            LogHelper.Info($"管道 {pipeCode} 阻塞", "PipeBlocked", "PipelineWatcher");
            return Task.CompletedTask;
        }
    }
}
