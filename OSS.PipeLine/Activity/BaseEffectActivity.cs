using System;
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

        #region 业务扩展方法

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <returns>
        /// (bool traffic_signal,TResult result)-（活动是否处理成功，业务结果）
        /// traffic_signal：
        /// traffic_signal：     
        ///     Green_Pass  - 流体自动流入后续管道
        ///     Yellow_Wait - 暂停执行，既不向后流动，也不触发Block。
        ///     Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </returns>
        protected abstract Task<(TrafficSignal traffic_signal, TResult result)> Executing();

        /// <summary>
        ///  阻塞调用扩展方法
        /// </summary>
        /// <param name="res"></param>
        /// <param name="blockedPipeCode">true-自身处理失败触发block，false-传递下个节点失败触发block</param>
        /// <returns></returns>
        protected virtual Task Block(TResult res,string blockedPipeCode)
        {
            return Task.CompletedTask;
        }

        #endregion


        #region 流体业务-启动

        /// <summary>
        /// 启动方法
        /// </summary>
        /// <returns></returns>
        public Task<TrafficResult> Execute()
        {
            return Execute(EmptyContext.Default);
        }

        #endregion

        #region 内部业务处理

        internal override async Task<TrafficResult> InterHandling(EmptyContext context)
        {
            var (traffic_signal, result) = await Executing();
            var trafficRes = new TrafficResult(traffic_signal, traffic_signal.signal == SignalFlag.Red_Block
                    ? PipeCode : string.Empty, result);

            await Watch(PipeCode, PipeType, WatchActionType.Executed, context, trafficRes);

            if (traffic_signal.signal == SignalFlag.Green_Pass)
            {
               return await ToNextThrough(result);
            } 

            if (traffic_signal.signal == SignalFlag.Red_Block)
            {
                await Block(result,PipeCode);
            }
            return trafficRes;
        }

        #endregion

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

        #region 业务扩展方法

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="para">当前活动上下文信息</param>
        /// <returns>
        /// (bool traffic_signal,TResult result)-（活动是否处理成功，业务结果）
        /// traffic_signal：
        /// traffic_signal：     
        ///     Green_Pass  - 流体自动流入后续管道
        ///     Yellow_Wait - 暂停执行，既不向后流动，也不触发Block。
        ///     Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </returns>
        protected abstract Task<(TrafficSignal traffic_signal, TResult result)> Executing(TInContext para);

        /// <summary>
        ///  阻塞调用扩展方法
        /// </summary>
        /// <param name="para"></param>
        /// <param name="res"></param>
        /// <param name="blockedPipeCode">true-自身处理失败触发block，false-传递下个节点失败触发block</param>
        /// <returns></returns>
        protected virtual Task Block(TInContext para, TResult res,string blockedPipeCode)
        {
            return Task.CompletedTask;
        }

        #endregion


        #region 流体业务处理

        internal override async Task<TrafficResult> InterHandling(TInContext context)
        {

            var (traffic_signal, result) = await Executing(context);
            var trafficRes = new TrafficResult(traffic_signal,
                traffic_signal.signal == SignalFlag.Red_Block ? PipeCode : string.Empty, result);

            await Watch(PipeCode, PipeType, WatchActionType.Executed, context, trafficRes);

            if (traffic_signal.signal == SignalFlag.Green_Pass)
            {
                return await ToNextThrough(result);
            }

            if (traffic_signal.signal == SignalFlag.Red_Block)
            {
                await Block(context, result, PipeCode);
            }

            return trafficRes;
        }

        #endregion
    }

}