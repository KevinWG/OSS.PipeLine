﻿#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventTask - 流体异步消息延缓中转通信管道部分
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion


using System.Threading.Tasks;

namespace OSS.EventFlow.Interface
{
    /// <summary>
    /// 异步消息延缓中转通信管道
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface ISuspendTunnel<in TData>
    {
        /// <summary>
        ///  数据存入阻塞通道
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<bool> Suspend(TData data);

        /// <summary>
        ///  数据由阻塞通道唤起
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Resume(TData data);
    }
}
