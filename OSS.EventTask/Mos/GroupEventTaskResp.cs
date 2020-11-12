#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventTask - 群组事件任务处理结果类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2019-04-07
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;

namespace OSS.EventTask.Mos
{
    [Flags]
    public enum GroupExecuteStatus
    {
        Complete = 1,
        Failed = 2,
        Revert = 4
    }

    internal class GroupExecuteResp<TData,TTRes>
        //where TTRes : class, new()
    {
        internal GroupExecuteStatus status { get; set; }
        internal IDictionary<EventTask<TData, TTRes>, EventTaskResp<TTRes>> TaskResults { get; set; }
    }

    public class GroupEventTaskResp<TTRes> :BaseTaskResp<GroupEventTaskMeta>
    {
        public IList<EventTaskResp<TTRes>> results
        {
            get;
          internal   set;
        }
    }
}
