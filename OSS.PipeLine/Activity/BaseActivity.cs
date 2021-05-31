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
    ///  活动基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseActivity<TContext> : BaseSinglePipe<TContext, TContext>
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
        /// <param name="contextData"></param>
        /// <returns></returns>
        protected abstract Task<bool> Executing(TContext contextData);
        
        internal override async Task<bool> InterHandling(TContext context)
        {
            var res = await Executing(context);
            if (res)
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
    public abstract class BaseEffectActivity<TContext, TResult> : BaseSinglePipe<TContext, TResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseEffectActivity() : base(PipeType.EffectActivity)
        {
        }

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="contextData"></param>
        /// <param name="isBlocked">是否阻塞当前数据流</param>
        /// <returns></returns>
        protected abstract Task<TResult> Executing(TContext contextData, ref bool isBlocked);

        internal override async Task<bool> InterHandling(TContext context)
        {
            var isBlocked = false;

            var res = await Executing(context, ref isBlocked);
            if (isBlocked)
            {
                await Block(context);
                return true;
            }

            await ToNextThrough(res);
            return false;
        }
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
