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

namespace OSS.Pipeline.Interface
{


    /// <summary>
    /// 
    /// </summary>
    public interface IActivity : IPipeAppender<Empty>, IPipeExecutor
    {
    }


    /// <summary>
    ///   活动接口（有输入无输出）
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IActivity<TContext> : IPipeAppender<TContext>, IPipeInputExecutor<TContext> // , IPipeExecutor<TContext, TContext>
    {
    }


    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TRes"></typeparam>
    public interface IActivity<TIn, TRes> : IPipeAppender<TIn>, IPipeExecutor<TIn, TRes>
    {
    }
    






    /// <typeparam name="TRes"></typeparam>
    public interface IEffectActivity<TRes> : IPipeAppender<TRes>, IPipeOutputExecutor<TRes>
    {
    }

    /// <typeparam name="TRes"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public interface IEffectActivity<in TContext,TRes> : IPipeAppender<TRes>, IPipeExecutor<TContext,TRes>
    {
    }
    
}
