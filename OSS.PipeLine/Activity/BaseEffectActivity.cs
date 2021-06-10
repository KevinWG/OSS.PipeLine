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
    public abstract class BaseEffectActivity<TResult> : BaseStraightPipe<EmptyContext, TResult>, IPipeExecutor
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseEffectActivity() : base(PipeType.EffectActivity)
        {
        }

        #region 业务扩展方法

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <returns>
        /// (bool traffic_signal,TResult result)-（活动是否处理成功，业务结果）
        /// traffic_signal：
        ///     False - 触发Block，业务流不再向后续管道传递。
        ///     True  - 流体自动流入后续管道
        /// </returns>
        protected abstract Task<(TrafficSignal traffic_signal, TResult result)> Executing();

        /// <summary>
        ///  阻塞调用扩展方法
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        protected virtual Task Block(TResult res)
        {
            return Task.CompletedTask;
        }

        #endregion


        #region 流体业务-启动

        /// <summary>
        /// 启动方法
        /// </summary>
        /// <returns></returns>
        public Task<TrafficSignal> Execute()
        {
            return Execute(EmptyContext.Default);
        }

        #endregion

        #region 内部业务处理

        internal override async Task<TrafficSignal> InterHandling(EmptyContext context)
        {
            var (traffic_signal, result) = await Executing();
            await Watch(PipeCode, PipeType, WatchActionType.Executed, context, result);

            if (traffic_signal == TrafficSignal.Green_Pass)
            {
                await ToNextThrough(result);
            }
            else if (traffic_signal == TrafficSignal.Red_Block)
            {
                await Block(result);
            }

            return traffic_signal;
        }

        #endregion

    }

    /// <summary>
    ///  主动触发执行活动组件基类
    ///       接收上下文，自身返回处理结果，且结果作为上下文传递给下一个节点
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class BaseEffectActivity<TInContext, TResult> : BaseStraightPipe<TInContext, TResult>, IPipeExecutor<TInContext>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseEffectActivity() : base(PipeType.EffectActivity)
        {
        }

        #region 业务扩展方法

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="para">当前活动上下文信息</param>
        /// <returns>
        /// (bool traffic_signal,TResult result)-（活动是否处理成功，业务结果）
        /// traffic_signal：
        ///     False - 触发Block，业务流不再向后续管道传递。
        ///     True  - 流体自动流入后续管道
        /// </returns>
        protected abstract Task<(TrafficSignal traffic_signal, TResult result)> Executing(TInContext para);

        /// <summary>
        ///  阻塞调用扩展方法
        /// </summary>
        /// <param name="para"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        protected virtual Task Block(TInContext para, TResult res)
        {
            return Task.CompletedTask;
        }

        #endregion


        #region 流体业务处理

        internal override async Task<TrafficSignal> InterHandling(TInContext context)
        {
            var (traffic_signal, result) = await Executing(context);
            await Watch(PipeCode, PipeType, WatchActionType.Executed, context, result);

            if (traffic_signal == TrafficSignal.Green_Pass)
            {
                await ToNextThrough(result);
            }
            else if (traffic_signal == TrafficSignal.Red_Block)
            {
                await Block(context,result);
            }

            return traffic_signal;
        }
        
        #endregion
    }

}