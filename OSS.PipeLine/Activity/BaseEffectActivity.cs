using System.Threading.Tasks;
using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;
using OSS.Pipeline.InterImpls.Watcher;

namespace OSS.Pipeline
{
    /// <summary>
    ///  主动触发执行活动组件基类
    ///       不接收上下文，自身返回处理结果，且结果作为上下文传递给下一个节点
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public abstract class BaseEffectActivity<TResult> : BaseStraightPipe<EmptyContext, TResult>, IActivity<TResult>
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
        /// <returns>
        /// (bool is_ok,TResult result)-（活动是否处理成功，业务结果）
        /// is_ok：
        ///     False - 触发Block，业务流不再向后续管道传递。
        ///     True  - 流体自动流入后续管道
        /// </returns>
        protected abstract Task<(bool is_ok, TResult result)> Executing();


        #region 流体业务-启动

        /// <summary>
        /// 启动方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<bool> Execute()
        {
            return InterStart(EmptyContext.Default);
        }

        #endregion



        internal override async Task<bool> InterHandling(EmptyContext context)
        {
            var (is_ok, result) = await Executing();
            await Watch(PipeCode, PipeType, WatchActionType.Executed, context, result);

            return is_ok && await ToNextThrough(result);
        }
    }

    /// <summary>
    ///  主动触发执行活动组件基类
    ///       接收上下文，自身返回处理结果，且结果作为上下文传递给下一个节点
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class BaseEffectActivity<TInContext, TResult> : BaseStraightPipe<TInContext, TResult>, IActivity<TInContext, TResult>
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
        /// <param name="para">当前活动上下文信息</param>
        /// <returns>
        /// (bool is_ok,TResult result)-（活动是否处理成功，业务结果）
        /// is_ok：
        ///     False - 触发Block，业务流不再向后续管道传递。
        ///     True  - 流体自动流入后续管道
        /// </returns>
        protected abstract Task<(bool is_ok, TResult result)> Executing(TInContext para);

        #region 流体业务处理

        internal override async Task<bool> InterHandling(TInContext context)
        {
            var (is_ok, result) = await Executing(context);

            await Watch(PipeCode, PipeType, WatchActionType.Executed, context, result);

            return is_ok && await ToNextThrough(result);
        }
        
        #endregion


        #region 流体业务-启动

        /// <summary>
        /// 启动方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<bool> Execute(TInContext para)
        {
            return InterStart(para);
        }

        #endregion
    }

}