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

    public interface IBasicActivity
    {

        /// <summary>
        /// 受影响的
        /// </summary>
        public bool IsEffected { get;  }

        /// <summary>
        ///  被动的
        /// </summary>
        public bool IsPassive { get;  }
    }


    /// <summary>
    /// 
    /// </summary>
    public interface IActivity : IPipeAppender<Empty>, IPipeExecutor, IBasicActivity
    {
    }


    /// <summary>
    ///   活动接口（有输入无输出）
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IActivity<TContext> : IPipeAppender<TContext>, IPipeInputExecutor<TContext>, IBasicActivity // , IPipeExecutor<TContext, TContext>
    {
    }


    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="THandleResult"></typeparam>
    public interface IActivity<TInContext, THandleResult> : IPipeAppender<TInContext>, IPipeExecutor<TInContext, THandleResult>, IBasicActivity
    {
    }
    






    /// <typeparam name="THandleResult"></typeparam>
    public interface IEffectActivity<THandleResult> : IPipeAppender<THandleResult>, IPipeOutputExecutor<THandleResult>, IBasicActivity
    {
    }

    /// <typeparam name="THandleResult"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public interface IEffectActivity<in TContext,THandleResult> : IPipeAppender<THandleResult>, IPipeExecutor<TContext,THandleResult>, IBasicActivity
    {
    }
    
}
