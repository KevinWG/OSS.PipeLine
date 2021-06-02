#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  外部动作活动
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion


using OSS.Pipeline.Interface;
using OSS.Pipeline.Mos;
using System.Threading.Tasks;

namespace OSS.Pipeline.Activity
{
    /// <summary>
    ///  被动触发执行活动组件基类
    ///      传入TFuncPara类型参数，且此参数作为后续上下文传递给下一个节点，自身返回处理结果但无影响
    /// </summary>
    /// <typeparam name="TFuncPara"></typeparam>
    /// <typeparam name="TFuncResult"></typeparam>
    public abstract class BaseFuncActivity<TFuncPara, TFuncResult> :
        BasePipe<EmptyContext, TFuncPara, TFuncPara>, IFuncActivity<TFuncPara, TFuncResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseFuncActivity() : base(PipeType.FuncActivity)
        {
        }

        /// <summary>
        ///  执行方法
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public async Task<TFuncResult> Execute(TFuncPara para)
        {
            var (is_ok, result) = await Executing(para);
            if (!is_ok)
            {
                await Block(para);
                return result;
            }

            await ToNextThrough(para);
            return result;
        }
        
        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="contextData">当前活动上下文信息</param>
        /// <returns>
        /// (bool is_ok,TResult result)-（活动是否处理成功，业务结果）
        /// is_ok：
        ///     False - 触发Block，业务流不再向后续管道传递。
        ///     True  - 流体自动流入后续管道
        /// </returns>
        protected abstract Task<(bool is_ok, TFuncResult result)> Executing(TFuncPara contextData);

        #region 内部的业务处理 

        internal override Task<bool> InterStart(EmptyContext context)
        {
            return Task.FromResult(true);
        }

        #endregion
    }


    /// <summary>
    ///  被动触发执行活动组件基类
    ///      传入TFuncPara类型参数，自身返回处理结果，且结果作为上下文传递给下一个节点
    /// </summary>
    /// <typeparam name="TFuncPara"></typeparam>
    /// <typeparam name="TFuncResult"></typeparam>
    public abstract class BaseFuncEffectActivity<TFuncPara, TFuncResult> :
        BasePipe<EmptyContext, TFuncPara, TFuncResult>, IFuncActivity<TFuncPara, TFuncResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseFuncEffectActivity() : base(PipeType.FuncActivity)
        {
        }
        
      
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async  Task<TFuncResult> Execute(TFuncPara data)
        {
            var (is_ok, result) = await Executing(data);
            if (!is_ok)
            {
                await Block(data);
                return result;
            }

            await ToNextThrough(result);
            return result;
        }

        
        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="contextData">当前活动上下文信息</param>
        /// <returns>
        /// (bool is_ok,TResult result)-（活动是否处理成功，业务结果）
        /// is_ok：
        ///     False - 触发Block，业务流不再向后续管道传递。
        ///     True  - 流体自动流入后续管道
        /// </returns>
        protected abstract Task<(bool is_ok, TFuncResult result)> Executing(TFuncPara contextData);


        #region 内部的业务处理 

        internal override Task<bool> InterStart(EmptyContext context)
        {
            return Task.FromResult(true);
        }

        #endregion

    }

    ///// <summary>
    /////  被动触发执行活动组件基类
    ///// </summary>
    ///// <typeparam name="TInContext"></typeparam>
    ///// <typeparam name="TFuncPara"></typeparam>
    ///// <typeparam name="TFuncResult"></typeparam>
    //public abstract class BaseFuncActivity<TInContext, TFuncPara, TFuncResult> :
    //    BasePipe<TInContext, TFuncPara, TFuncResult>, IFuncActivity<TFuncPara, TFuncResult>
    //{
    //    /// <summary>
    //    /// 外部Action活动基类
    //    /// </summary>
    //    protected BaseFuncActivity() : base(PipeType.FuncActivity)
    //    {
    //    }

    //    /// <summary>
    //    ///  执行方法
    //    /// </summary>
    //    /// <param name="data"></param>
    //    /// <returns></returns>
    //    public async Task<TFuncResult> Execute(TFuncPara data)
    //    {
    //        var (is_ok, result) = await Executing(data);
    //        if (!is_ok)
    //        {
    //            await Block(data);
    //            return result;
    //        }

    //        await ToNextThrough(result);
    //        return result;
    //    }


    //    /// <summary>
    //    ///  具体执行扩展方法
    //    /// </summary>
    //    /// <param name="contextData">当前活动上下文信息</param>
    //    /// <returns>
    //    /// (bool is_ok,TResult result)-（活动是否处理成功，业务结果）
    //    /// is_ok：
    //    ///     False - 触发Block，业务流不再向后续管道传递。
    //    ///     True  - 流体自动流入后续管道
    //    /// </returns>
    //    protected abstract Task<(bool is_ok, TFuncResult result)> Executing(TFuncPara contextData);

    //    /// <summary>
    //    ///  消息进入通知
    //    /// </summary>
    //    /// <param name="data"></param>
    //    /// <returns></returns>
    //    protected virtual Task<bool> Notice(TInContext data)
    //    {
    //        return Task.FromResult(true);
    //    }


    //    #region 管道内部业务处理

    //    internal override Task<bool> InterStart(TInContext context)
    //    {
    //        return Task.FromResult(true);
    //    }

    //    internal override Task<bool> InterHandling(TFuncPara context)
    //    {
    //        return Task.FromResult(true);
    //    }

    //    #endregion
    //}

}
