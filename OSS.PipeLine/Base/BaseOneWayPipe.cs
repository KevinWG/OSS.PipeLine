﻿#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

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
using OSS.Pipeline.Base.Base;

namespace OSS.Pipeline.Base
{
    /// <summary>
    ///  管道执行基类（单入类型）
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseOneWayPipe<TContext> : BasePipe<TContext, TContext, TContext, TContext>, IPipeInputExecutor<TContext>
    {
        /// <inheritdoc />
        protected BaseOneWayPipe(string pipeCode, PipeType pipeType) : base(pipeCode,pipeType)
        {
        }
        
        #region 外部业务扩展

        /// <summary>
        /// 外部执行方法 - 启动入口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(TContext context)
        {
            return  InterProcess(context,string.Empty);
        }
        
        #endregion



        #region 流体内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficResult> InterPreCall(TContext context, string prePipeCode)
        {
            await Watch(PipeCode, PipeType, WatchActionType.PreCall, context).ConfigureAwait(false);
            var tRes = await InterProcess(context,prePipeCode);
            return tRes.ToResult();
        }

        /// <inheritdoc />
        internal override async Task<TrafficResult<TContext, TContext>> InterProcessHandling(TContext context, string prePipeCode)
        {
            var trafficRes   = await InterProcessPackage(context,prePipeCode);
            await Watch(PipeCode, PipeType, WatchActionType.Executed, context, trafficRes.ToWatchResult()).ConfigureAwait(false);
            
            if (trafficRes.signal == SignalFlag.Red_Block)
            {
                await InterBlock(context, trafficRes);
            }
            return trafficRes;
        }

        //internal override Task<TrafficSignal<TContext>> InterBeforProcessing(TContext context)
        //{
        //    throw new System.NotImplementedException();
        //}

        #endregion
        
        #region 管道初始化

        /// <inheritdoc />
        internal override void InterInitialContainer(IPipeLine flowContainer)
        {
            LineContainer = flowContainer;
            WatchProxy    = flowContainer.GetProxy();
        }

        #endregion

        #region 管道路由

        //  节点本身是一个独立的结束节点
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