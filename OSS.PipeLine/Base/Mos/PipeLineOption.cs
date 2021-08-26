#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  外部动作活动
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

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
        public IPipeLineWatcher Watcher { get; set; }

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