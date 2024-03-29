﻿#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  连接基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Pipeline.Base;

namespace OSS.Pipeline;

/// <summary>
/// 消息转化基类
/// </summary>
/// <typeparam name="TMsg">消息具体类型</typeparam>
public class MsgEnumerator<TMsg> : BaseThreeWayPipe<IEnumerable<TMsg>, Empty, TMsg>
{
    private readonly Func<IEnumerable<TMsg>, IEnumerable<TMsg>> _msgFilter = null;

    /// <summary>
    /// 消息转化基类 
    /// </summary>
    public MsgEnumerator(Func<IEnumerable<TMsg>, IEnumerable<TMsg>> msgFilter = null, string pipeCode = null) : base(
        pipeCode, PipeType.MsgEnumerator)
    {
        _msgFilter = msgFilter;
    }

    /// <summary>
    ///  过滤处理消息
    /// </summary>
    /// <param name="msgList"></param>
    /// <returns></returns>
    protected virtual IEnumerable<TMsg> Filter(IEnumerable<TMsg> msgList)
    {
        return _msgFilter != null ? _msgFilter(msgList) : msgList;
    }

    #region 管道内部业务处理

    /// <inheritdoc />
    internal override async Task<TrafficSignal<Empty, TMsg>> InterProcessingAndDistribute(IEnumerable<TMsg> msgList)
    {
        var filterMsgList = Filter(msgList);
        if (filterMsgList == null || !filterMsgList.Any())
            throw new ArgumentNullException(nameof(msgList), "无消息可以枚举!");

        var trafficRes = await InterWatchProcessing(filterMsgList);

        if (trafficRes.signal == SignalFlag.Red_Block)
            await InterWatchBlock(filterMsgList, trafficRes);

        return trafficRes;
    }

    /// <inheritdoc />
    internal override async Task<TrafficSignal<Empty, TMsg>> InterProcessing(IEnumerable<TMsg> msgs)
    {
        var parallelTasks = msgs.Select(ToNextThrough);

        return (await Task.WhenAll(parallelTasks)).Any(r => r.signal == SignalFlag.Green_Pass)
            ? new TrafficSignal<Empty, TMsg>(SignalFlag.Green_Pass, Empty.Default, default)
            : new TrafficSignal<Empty, TMsg>(SignalFlag.Red_Block, Empty.Default, default, "所有分支运行失败！");
    }


    #endregion
}

