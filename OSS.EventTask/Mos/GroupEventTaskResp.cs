using System;
using System.Collections.Generic;
using System.Linq;
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
        internal IDictionary<IEventTask<TData, TTRes>, EventTaskResp<TTRes>> TaskResults { get; set; }
    }

    public class GroupEventTaskResp<TTRes> :BaseTaskResp<GroupEventTaskMeta,TTRes>
    {
      
    }
}
