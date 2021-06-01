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


using OSS.PipeLine.Mos;
using System.Threading.Tasks;

namespace OSS.PipeLine.Activity
{
    /// <summary>
    ///  被动触发执行活动组件基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class BaseFuncActivity<TContext, TResult> : BaseSinglePipe<TContext>
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
        ///  Action执行方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<TResult> Action(TContext data)
        {
            var isOK = false;

            var res = await Executing(data, ref isOK);
            if (!isOK)
            {
                await Block(data);
                return res;
            }

            await ToNextThrough(data);
            return res;
        }


        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="contextData">当前活动上下文信息</param>
        /// <param name="isOk">
        /// 处理结果 - 决定是否阻塞当前数据流
        /// False - 触发Block，业务流不再向后续管道传递。
        /// True  - 流体自动流入后续管道
        /// </param>
        /// <returns></returns>
        protected abstract Task<TResult> Executing(TContext contextData, ref bool isOk);
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


    /// <inheritdoc />
    public abstract class BaseEffectFuncActivity<TContext, TResult> : BasePipe<TContext, TResult>
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
        public async Task<TResult> Action(TContext data)
        {
            var isOK = false;

            var res = await Executing(data, ref isOK);
            if (!isOK)
            {
                await Block(data);
                return res;
            }

            await ToNextThrough(res);
            return res;
        }



        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="contextData">当前活动上下文信息</param>
        /// <param name="isOk">
        /// 处理结果 - 决定是否阻塞当前数据流
        /// False - 触发Block，业务流不再向后续管道传递。
        /// True  - 流体自动流入后续管道
        /// </param>
        /// <returns></returns>
        protected abstract Task<TResult> Executing(TContext contextData, ref bool isOk);

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
