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
    /// <typeparam name="TPassiveResult"></typeparam>
    public interface IPassiveActivity<TPassivePara, TPassiveResult> : IPipeAppender<TPassivePara>,IPipeExecutor<TPassivePara, TPassiveResult>
    {
    }

    /// <typeparam name="TPassivePara"></typeparam>
    /// <typeparam name="TPassiveResult"></typeparam>
    public interface IPassiveEffectActivity<in TPassivePara, TPassiveResult> : IPipeAppender<TPassiveResult>, IPipeExecutor<TPassivePara, TPassiveResult>
    {
    }
}
