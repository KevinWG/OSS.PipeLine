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

using System.Threading.Tasks;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Activity
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
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract Task<bool> Executing(TContext data);


        internal override async Task<bool> InterHandling(TContext context)
        {
            var res = await Executing(context);
            if (res)
            {
                await Block(context);
                return res;
            }

            await ToNextThrough(context);
            return res;
        }
    }


    /// <summary>
    ///  活动基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseEffectActivity<TContext, TResult> : BaseSinglePipe<TContext, TResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseEffectActivity() : base(PipeType.EffectActivity)
        {
        }


        protected BaseEffectActivity(PipeType type) : base(type)
        {
        }

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isBlocked"></param>
        /// <returns></returns>
        protected abstract Task<TResult> Executing(TContext data, out bool isBlocked);


        internal override async Task<bool> InterHandling(TContext context)
        {
            var res= await Executing(context, out var isBlocked);
            if (isBlocked)
            {
                await Block(context);
                return isBlocked;
            }

            await ToNextThrough(res);
            return isBlocked;
        }
    }


    /// <summary>
    ///  空上下文
    /// </summary>
    public class EmptyContext //: IPipeContext
    {

    }

    /// <summary>
    /// 空活动
    /// </summary>
    public class EmptyActivity : BaseActivity<EmptyContext>
    {
        protected override Task<bool> Executing(EmptyContext data)
        {
            return Task.FromResult(true);
        }
    }


 

}
