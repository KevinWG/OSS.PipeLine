﻿#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  管道部分组成基类
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
    /// 管道组成基类
    /// </summary>
    public abstract class BasePipePart : IPipeInitiator
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pipeCode"></param>
        /// <param name="pipeType"></param>
        protected BasePipePart(string pipeCode, PipeType pipeType)
        {
            PipeType = pipeType;
            PipeCode = string.IsNullOrEmpty(pipeCode) ? GetType().Name : pipeCode;
        }

        /// <summary>
        ///  管道类型
        /// </summary>
        public PipeType PipeType { get; internal set; }

        /// <summary>
        ///  管道编码
        ///  默认等于  this.GetType().Name
        /// </summary>
        public string PipeCode { get; set; }

        /// <summary>
        ///  流容器
        /// </summary>
        protected IPipeLine LineContainer { get; set; }



        #region 管道监控

        internal PipeWatcherProxy WatchProxy { get; set; }

        internal Task Watch(string pipeCode, PipeType pipeType, WatchActionType actionType, object para,
            WatchResult res)
        {
            if (WatchProxy != null)
            {
                return WatchProxy.Watch(new WatchDataItem()
                {
                    PipeCode   = pipeCode,
                    PipeType   = pipeType,
                    ActionType = actionType,

                    Para   = para,
                    Result = res
                });
            }

            return Task.CompletedTask;
        }

        internal Task Watch(string pipeCode, PipeType pipeType, WatchActionType actionType, object para)
        {
            return Watch(pipeCode, pipeType, actionType, para, default);
        }

        #endregion

        #region 内部初始化(容器和路由)

        /// <summary>
        ///  内部处理流容器初始化赋值
        /// </summary>
        /// <param name="containerFlow"></param>
        internal abstract void InterInitialContainer(IPipeLine containerFlow);

        void IPipeInitiator.InterInitialContainer(IPipeLine containerFlow)
        {
            InterInitialContainer(containerFlow);
        }

        /// <summary>
        ///  内部处理流的路由信息
        /// </summary>
        /// <returns></returns>
        internal abstract void InterFormatLink(string prePipeCode, bool isSelf = false);

        void IPipeInitiator.InterFormatLink(string prePipeCode, bool isSelf)
        {
            InterFormatLink(prePipeCode, isSelf);
        }

        #endregion
    }

    /// <summary>
    /// 管道进口基类
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    public abstract class BaseInPipePart<TIn> : BasePipePart , IPipeInPart<TIn>
    {
        /// <inheritdoc />
        protected BaseInPipePart(string pipeCode, PipeType pipeType) : base(pipeCode, pipeType)
        {
        }
        
        #region 管道的业务处理

        /// <summary>
        ///  内部管道 -- 唤起
        /// </summary>
        internal abstract Task<TrafficResult> InterPreCall(TIn context);

        async Task<TrafficResult> IPipeInPart<TIn>.InterWatchPreCall(TIn context)
        {
            await Watch(PipeCode, PipeType, WatchActionType.PreCall, context).ConfigureAwait(false);
            return await InterPreCall(context);
        }
        #endregion

 
    }




}