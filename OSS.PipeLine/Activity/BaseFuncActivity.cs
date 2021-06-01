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
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class BaseFuncActivity<TContext, TResult> : BaseSinglePipe<TContext>, IFuncActivity<TContext, TResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseFuncActivity() : base(PipeType.FuncActivity)
        {
        }

        internal override Task<bool> InterHandling(TContext context)
        {
            return Notice(context);
        }


        /// <summary>
        ///  执行方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<TResult> Execute(TContext data)
        {
            var (is_ok, result) = await Executing(data);
            if (!is_ok)
            {
                await Block(data);
                return result;
            }
            await ToNextThrough(data);
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
        protected abstract Task<(bool is_ok,TResult result)> Executing(TContext contextData);

        /// <summary>
        ///  消息进入通知
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task<bool> Notice(TContext data)
        {
            return Task.FromResult(true);
        }
    }


    /// <inheritdoc />
    public abstract class BaseEffectFuncActivity<TContext, TResult> : BasePipe<TContext, TResult>, IFuncActivity<TContext, TResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseEffectFuncActivity() : base(PipeType.FuncEffectActivity)
        {
        }
        

        internal override Task<bool> InterHandling(TContext context)
        {
            return Notice(context);
        }


        /// <summary>
        ///  Action执行方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<TResult> Execute(TContext data)
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
        protected abstract Task<(bool is_ok, TResult result)> Executing(TContext contextData);

        /// <summary>
        ///  消息进入通知
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual Task<bool> Notice(TContext data)
        {
            return Task.FromResult(true);
        }
    }
}
