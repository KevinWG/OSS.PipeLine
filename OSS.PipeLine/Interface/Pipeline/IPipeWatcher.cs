using System.Threading.Tasks;

namespace OSS.Pipeline.Interface
{
    /// <summary>
    /// 管道监视器
    /// </summary>
    public interface IPipeWatcher
    {
        /// <summary>
        ///  进入当前管道启动
        /// </summary>
        public Task<bool> Starting(string pipeCode, PipeType pipeType, object data);
        
        /// <summary>
        ///  执行完成
        /// </summary>
        public Task<bool> Excuted(string pipeCode, PipeType pipeType, object data, object result);

        /// <summary>
        ///  管道阻塞
        /// </summary>
        public Task<bool> Blocked(string pipeCode, PipeType pipeType, object data);
    }


}
