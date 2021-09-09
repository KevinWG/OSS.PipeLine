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
using OSS.Pipeline.Activity.Base;
using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline
{
    /// <summary>
    ///  主动触发执行活动组件基类
    ///       不接收上下文，自身返回处理结果，且结果作为上下文传递给下一个节点
    /// </summary>
    /// <typeparam name="THandleResult"></typeparam>
    public abstract class BaseEffectActivity<THandleResult> : BaseThreeWayPipe<Empty,THandleResult, THandleResult>, IEffectActivity<THandleResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseEffectActivity( string pipeCode = null) : base(pipeCode,PipeType.Activity)
        {
        }
        /// <inheritdoc />
        public bool IsEffected => true;

        /// <inheritdoc />
        public bool IsPassive => false;


        #region 流体业务-启动

        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        public Task<THandleResult> Execute()
        {
            return Execute(Empty.Default);
        }

        #endregion

        #region 业务扩展方法

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <returns>
        ///  -（活动是否处理成功，业务结果）
        /// traffic_signal：
        /// traffic_signal：     
        ///     Green_Pass  - 流体自动流入后续管道
        ///     Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///     Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </returns>
        protected abstract Task<TrafficSignal<THandleResult>> Executing();

        #endregion

        #region 流体内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficResult<THandleResult, THandleResult>> InterProcessPackage(Empty context, string prePipeCode)
        {
            var trafficRes = await Executing();
            return new TrafficResult<THandleResult, THandleResult>(trafficRes,
                trafficRes.signal == SignalFlag.Red_Block ? PipeCode : string.Empty, trafficRes.result);
        }


        #endregion
    }

    /// <summary>
    ///  主动触发执行活动组件基类
    ///       接收上下文，自身返回处理结果，且结果作为上下文传递给下一个节点
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="THandleResult"></typeparam>
    public abstract class BaseEffectActivity<TContext, THandleResult> : BaseThreeWayActivity<TContext,THandleResult, THandleResult>, IEffectActivity<TContext, THandleResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseEffectActivity(string pipeCode = null) : base(pipeCode,PipeType.Activity,true,false)
        {
        }
        
        #region 流体内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficResult<THandleResult, THandleResult>> InterProcessPackage(TContext context, string prePipeCode)
        {
            var trafficRes = await Executing(context);
            return new TrafficResult<THandleResult, THandleResult>(trafficRes,
                trafficRes.signal == SignalFlag.Red_Block ? PipeCode : string.Empty, trafficRes.result);
        }

        #endregion
    }

}