#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  活动基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using OSS.PipeLine.Mos;
using System.Threading.Tasks;

namespace OSS.PipeLine.Activity
{

    /// <summary>
    ///  主动触发执行活动组件基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseActivity<TContext> : BaseSinglePipe<TContext>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseActivity() : base(PipeType.Activity)
        {
        }

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="contextData">当前活动上下文信息</param>
        /// <returns>
        /// 处理结果
        /// False - 触发Block，业务流不再向后续管道传递。
        /// True  - 流体自动流入后续管道
        /// </returns>
        protected abstract Task<bool> Executing(TContext contextData);
        
        internal override async Task<bool> InterHandling(TContext context)
        {
            var res = await Executing(context);
            if (!res)
            {
                await Block(context);
                return true;
            }

            await ToNextThrough(context);
            return false;
        }
    }

    /// <summary>
    ///  活动基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class BaseEffectActivity<TContext, TResult> : BasePipe<TContext, TResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseEffectActivity() : base(PipeType.EffectActivity)
        {
        }

        internal override async Task<bool> InterHandling(TContext context)
        {
            var (is_ok, result) = await Executing(context);
            if (!is_ok)
            {
                await Block(context);
                return is_ok;
            }

            await ToNextThrough(result);
            return is_ok;
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
        protected abstract Task<(bool is_ok, TResult result)> Executing(TContext contextData);
    }

    /// <summary>
    /// 空活动
    /// </summary>
    public class EmptyActivity<TContext> : BaseActivity<TContext>
    {
        /// <summary>
        ///  执行空操作
        /// </summary>
        /// <param name="contextData"></param>
        /// <returns></returns>
        protected override Task<bool> Executing(TContext contextData)
        {
            return Task.FromResult(true);
        }
    }
    

}
