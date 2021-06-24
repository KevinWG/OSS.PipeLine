#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  流体监视器
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

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
        public Task<bool> Starting(string pipeCode, PipeType pipeType, object input);
        
        /// <summary>
        ///  执行完成
        /// </summary>
        public Task<bool> Excuted(string pipeCode, PipeType pipeType, object input, WatchResult watchResult);

        /// <summary>
        ///  管道阻塞
        /// </summary>
        public Task<bool> Blocked(string pipeCode, PipeType pipeType, object input, WatchResult watchResult);
    }


}
