#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 流体基础管道部分
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.Pipeline.Interface;
using OSS.Pipeline.InterImpls.Watcher;

namespace OSS.Pipeline.Base
{
    /// <summary>
    ///  管道执行基类（单入类型）
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    public abstract class BaseOneWayPipe<TInContext> : BaseInPipePart<TInContext>,IPipeExecutor<TInContext,TInContext>
    {
        /// <inheritdoc />
        protected BaseOneWayPipe(PipeType pipeType) : base(pipeType)
        {
        }
        
        #region 外部业务扩展

        /// <summary>
        /// 外部执行方法 - 启动入口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<TInContext> Execute(TInContext context)
        {
            return (await InterProcess(context)).result;
        }


        #endregion

        #region 流体内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficResult> InterPreCall(TInContext context)
        {
            await Watch(PipeCode, PipeType, WatchActionType.Starting, context);
            var tRes = await InterProcess(context);
            return tRes.ToResult();
        }


        /// <summary>
        ///  内部管道 -- （2）执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal Task<TrafficResult<TInContext, TInContext>> InterProcess(TInContext context)
        {
            return InterProcessHandling(context);
        }

        /// <summary>
        ///  内部管道 -- （3）执行 - 控制流转
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal async Task<TrafficResult<TInContext, TInContext>> InterProcessHandling(TInContext context)
        {
            var trafficRes = await InterProcessPackage(context);
            await Watch(PipeCode, PipeType, WatchActionType.Executed, context, trafficRes.ToWatchResult());
            
            if (trafficRes.signal == SignalFlag.Red_Block)
            {
                await InterBlock(context, trafficRes);
            }
            return trafficRes;
        }

        /// <summary>
        ///  内部管道 -- （4）执行 - 组装业务结果
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal abstract Task<TrafficResult<TInContext, TInContext>> InterProcessPackage(TInContext context);


        /// <summary>
        ///  管道堵塞
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tRes"></param>
        /// <returns></returns>
        internal virtual async Task InterBlock(TInContext context, TrafficResult<TInContext, TInContext> tRes)
        {
            await Watch(PipeCode, PipeType, WatchActionType.Blocked, context, tRes.ToWatchResult());
            await Block(context, tRes);
        }

        /// <summary>
        ///  管道堵塞
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tRes"></param>
        /// <returns></returns>
        protected virtual Task Block(TInContext context, TrafficResult<TInContext, TInContext> tRes)
        {
            return Task.CompletedTask;
        }
        #endregion

    
        #region 管道初始化

        internal override void InterInitialContainer(IPipeLine flowContainer)
        {
            LineContainer = flowContainer;
            WatchProxy    = flowContainer.GetProxy();
        }

        #endregion

        #region 管道路由
        //  消息发布节点本身是一个独立的结束节点
        internal override PipeRoute InterToRoute(bool isFlowSelf = false)
        {
            var pipe = new PipeRoute()
            {
                pipe_code = PipeCode,
                pipe_type = PipeType
            };
            return pipe;
        }

        #endregion
     
    }
}