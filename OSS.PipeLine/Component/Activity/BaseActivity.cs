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
using OSS.Pipeline.Activity.Base;
using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline
{
    /// <summary>
    /// 主动触发执行活动组件基类(不接收上下文)
    /// </summary>
    public abstract class BaseActivity : BaseThreeWayPipe<Empty,Empty, Empty>, IActivity
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
        /// <returns>
        /// 处理结果
        /// traffic_signal：     
        ///     Green_Pass  - 流体自动流入后续管道
        ///     Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///     Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </returns>
        protected abstract Task<TrafficSignal> Executing();
        

        #region 流体业务-启动

        /// <summary>
        /// 启动方法
        /// </summary>
        /// <returns></returns>
        public Task Execute()
        {
            return Execute(Empty.Default);
        }

        #endregion

        #region 流体内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficResult<Empty, Empty>> InterProcessPackage(Empty context,string prePipeCode)
        {
            var trafficRes = await Executing();
            return new TrafficResult<Empty, Empty>(trafficRes,
                trafficRes.signal == SignalFlag.Red_Block ? PipeCode : string.Empty, context, context);
        }

        #endregion
    }

    /// <summary>
    ///  主动触发执行活动组件基类
    ///    接收输入上下文，且此上下文继续传递下一个节点
    /// </summary>
    /// <typeparam name="TContext">输入输出上下文</typeparam>
    public abstract class BaseActivity<TContext> : BaseThreeWayPipe<TContext,TContext, TContext>, IActivity<TContext>
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
        /// <param name="para">当前活动上下文（会继续传递给下一个节点）</param>
        /// <returns>
        /// 处理结果
        /// traffic_signal：     
        ///     Green_Pass  - 流体自动流入后续管道
        ///     Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///     Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </returns>
        protected abstract Task<TrafficSignal> Executing(TContext para);

        /// <summary>
        /// 启动入口
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public new Task Execute(TContext para)
        {
            return base.Execute(para);
        }

        #region 流体内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficResult<TContext, TContext>> InterProcessPackage(TContext context, string prePipeCode)
        {
            var trafficRes = await Executing(context);
            return new TrafficResult<TContext, TContext>(trafficRes,
                trafficRes.signal == SignalFlag.Red_Block ? PipeCode : string.Empty, context, context);
        }

   
        #endregion
    }

    /// <summary>
    ///  主动触发执行活动组件基类
    ///    接收输入上下文，且此上下文继续传递下一个节点
    /// </summary>
    /// <typeparam name="TContext">输入输出上下文</typeparam>
    /// <typeparam name="THandleResult"></typeparam>
    public abstract class BaseActivity<TContext, THandleResult> : BaseThreeWayActivity<TContext, THandleResult, TContext>
        , IActivity<TContext, THandleResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseActivity() : base(PipeType.Activity)
        {
        }
  

        #region 流体内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficResult<THandleResult, TContext>> InterProcessPackage(TContext context, string prePipeCode)
        {
            var trafficRes = await Executing(context);
            return new TrafficResult<THandleResult, TContext>(trafficRes,
                trafficRes.signal == SignalFlag.Red_Block ? PipeCode : string.Empty, context);
        }

        #endregion
    }
}
