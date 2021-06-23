#region Copyright (C) 2021 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 流体基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2021-02-09
*       
*****************************************************************************/

#endregion

using OSS.Pipeline.InterImpls.Watcher;
using System.Threading.Tasks;

namespace OSS.Pipeline.Interface
{
    /// <summary>
    ///  管道基础接口
    /// </summary>
    public interface IPipe
    {
        /// <summary>
        ///  管道类型
        /// </summary>
        PipeType PipeType { get; }

        /// <summary>
        ///  管道编码
        /// </summary>
        public string PipeCode { get; set; }
    }

    /// <summary>
    ///  管道基础接口
    /// </summary>
    public interface IPipeLine : IPipe
    {
        /// <summary>
        ///  开始管道
        /// </summary>
        IPipe StartPipe { get; }
        
        /// <summary>
        ///  结束管道
        /// </summary>
        IPipe EndPipe { get; }

        /// <summary>
        ///   获取路由
        /// </summary>
        /// <returns></returns>
        PipeRoute ToRoute();
        

        internal PipeWatcherProxy GetProxy();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    public interface IPipeLine<in TInContext> : IPipeLine
    {
        /// <summary>
        ///  启动执行方法
        /// </summary>
        /// <returns></returns>
        Task<TrafficResult> Execute(TInContext context);
    }

}
