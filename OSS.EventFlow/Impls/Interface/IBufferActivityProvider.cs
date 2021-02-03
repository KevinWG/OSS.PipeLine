#region Copyright (C) 2021 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  默认缓冲活动提供者接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2021-01-24
*       
*****************************************************************************/

#endregion

using OSS.EventFlow.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Activity.Interface
{
    /// <summary>
    ///  默认活动的提供者
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IBufferActivityProvider<in TContext>:IActivityProvider<TContext>,IBufferPush<TContext>
        where TContext : IFlowContext
    {
    }
}