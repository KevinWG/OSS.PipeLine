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
        public Task<bool> Starting();
        
        /// <summary>
        ///  执行完成
        /// </summary>
        public Task<bool> Excuted();

        /// <summary>
        ///  管道阻塞
        /// </summary>
        public Task<bool> Blocked();
    }


}
