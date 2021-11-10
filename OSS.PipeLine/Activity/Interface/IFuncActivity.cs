#region Copyright (C) 2021 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 被动委托活动接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2021-6-1
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;

namespace OSS.Pipeline.Interface
{

    /// <typeparam name="TPassivePara"></typeparam>
    /// <typeparam name="TPassiveRes"></typeparam>
    public interface IPassiveActivity<TPassivePara, TPassiveRes> : IPipeAppender<TPassivePara>,IPipeExecutor<TPassivePara, TPassiveRes>
    {
    }

    /// <typeparam name="TPassivePara"></typeparam>
    /// <typeparam name="TPassiveRes"></typeparam>
    public interface IPassiveEffectActivity<in TPassivePara, TPassiveRes> : IPipeAppender<TPassiveRes>, IPipeExecutor<TPassivePara, TPassiveRes>
    {
    }
}
