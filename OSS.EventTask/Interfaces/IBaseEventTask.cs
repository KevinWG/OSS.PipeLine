#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventTask - 事件任务接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2019-04-07
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.EventTask.Mos;

namespace OSS.EventTask.Interfaces
{
    public interface IEventTask<in TTData, TTRes> : IBaseEventTask<TTData, TTRes>
        //where TTRes : class, new()
    {
        Task<bool> Revert(TTData data);
    }

    public interface IBaseEventTask<in TTData, TTRes>//: IMeta<EventTaskMeta>
        //where TTRes :class, new()
    {
        EventElementType OwnerType { get; }
        Task<EventTaskResp<TTRes>> Process(TTData data);
        Task<EventTaskResp<TTRes>> Process(TTData data,int triedTimes);
    }




}
