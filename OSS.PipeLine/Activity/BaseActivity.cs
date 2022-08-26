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

using OSS.Pipeline.Base;
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <summary>
    /// 主动触发执行活动组件基类(不接收上下文)
    /// </summary>
    public abstract class BaseActivity : BaseThreeWayPipe<Empty,Empty, Empty> //, IActivity
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseActivity(string pipeCode=null) : base(pipeCode, PipeType.Activity)
        {
        }

        #region 部具体执行扩展
        
        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <returns></returns>
        protected abstract Task<TrafficSignal> Executing();

        #endregion


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
        internal override async Task<TrafficSignal<Empty, Empty>> InterProcessing(Empty context)
        {
            var trafficRes = await Executing();
            return new TrafficSignal<Empty, Empty>(trafficRes.signal, context, context);
        }

        #endregion
    }

    /// <summary>
    ///  主动触发执行活动组件基类
    ///    接收输入上下文，且此上下文继续传递下一个节点
    /// </summary>
    /// <typeparam name="TIn">输入输出上下文</typeparam>
    public abstract class BaseActivity<TIn> : BaseThreeWayPipe<TIn,Empty, TIn> //, IActivity<TContext>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseActivity(string pipeCode = null) : base(pipeCode,PipeType.Activity)
        {
        }

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="para">当前活动上下文（会继续传递给下一个节点）</param>
        /// <returns>  </returns>
        protected abstract Task<TrafficSignal> Executing(TIn para);

        /// <summary>
        /// 启动入口
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public new Task Execute(TIn para)
        {
            return base.Execute(para);
        }

        #region 流体内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficSignal<Empty, TIn>> InterProcessing(TIn req)
        {
            var trafficRes = await Executing(req);
            return new TrafficSignal<Empty, TIn>(trafficRes.signal, Empty.Default, req);
        }

        #endregion
    }

    /// <summary>
    ///  主动触发执行活动组件基类
    ///    接收输入上下文，且此上下文继续传递下一个节点
    /// </summary>
    /// <typeparam name="TIn">输入输出上下文</typeparam>
    /// <typeparam name="TRes"></typeparam>
    public abstract class BaseActivity<TIn, TRes> : BaseThreeWayPipe<TIn, TRes, TIn>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseActivity(string pipeCode = null) : base(pipeCode,PipeType.Activity)
        {
        }

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="para">当前活动上下文（会继续传递给下一个节点）</param>
        /// <returns>  </returns>
        protected abstract Task<TrafficSignal<TRes>> Executing(TIn para);


        #region 流体内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficSignal<TRes, TIn>> InterProcessing(TIn req)
        {
            var trafficRes = await Executing(req);
            return new TrafficSignal<TRes, TIn>(trafficRes.signal,trafficRes.result, req, trafficRes.msg);
        }

        #endregion
    }


    /// <summary>
    ///  主动触发执行活动组件基类
    ///    接收输入上下文，且此上下文继续传递下一个节点
    /// </summary>
    /// <typeparam name="TIn">输入输出上下文</typeparam>
    /// <typeparam name="TRes"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public abstract class BaseActivity<TIn, TRes,TOut> : BaseThreeWayPipe<TIn, TRes, TOut>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseActivity(string pipeCode = null) : base(pipeCode, PipeType.Activity)
        {
        }

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="para">当前活动上下文（会继续传递给下一个节点）</param>
        /// <returns>  </returns>
        protected abstract Task<TrafficSignal<TRes, TOut>> Executing(TIn para);


        #region 流体内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficSignal<TRes, TOut>> InterProcessing(TIn req)
        {
            var trafficRes = await Executing(req);
            return new TrafficSignal<TRes, TOut>(trafficRes.signal, trafficRes.result, trafficRes.output, trafficRes.msg);
        }

        #endregion
    }
}
