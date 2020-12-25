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


using System.Threading.Tasks;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Activity
{
    /// <summary>
    /// 外部Action活动基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class BaseActionActivity<TContext,TResult> : BaseSinglePipe<TContext, TContext>
        where TContext : IFlowContext
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseActionActivity() : base(PipeType.Activity)
        {
        }

        /// <summary>
        ///  Action具体执行子类扩展方法
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isBlocked"></param>
        /// <returns></returns>
        protected abstract Task<TResult> ActionExecuting(TContext data,out bool isBlocked);

        /// <summary>
        ///  Action执行方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<TResult> Action(TContext data)
        {
            var res = await ActionExecuting(data,out var isBlocked);
            if (isBlocked)
            {
               await Block(data);
               return res;
            }

            await ToNextThrough(data);
            return res;
        }

        /// <summary>
        ///  消息进入通知
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual Task<bool> Notice(TContext data)
        {
            return Task.FromResult(true);
        }

        internal override Task<bool> Through(TContext context)
        {
            return Notice(context);
        }
    }
}
