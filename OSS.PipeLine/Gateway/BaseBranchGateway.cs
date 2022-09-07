#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  流体的分支网关基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-27
*       
*****************************************************************************/

#endregion

using OSS.Pipeline.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using OSS.Pipeline.Base;
using OSS.Pipeline.Gateway.InterImpls;
using OSS.Pipeline.InterImpls;

namespace OSS.Pipeline
{

    public interface IBranchGateway<out TContext> : IPipeAppender<TContext>
    {
        internal void SetCondition(IPipeMeta pipe, Func<TContext, bool> condition);
    }

    /// <summary>
    /// 流体的分支网关基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseBranchGateway<TContext> : BaseThreeWayPipe<TContext, TContext, TContext>, IBranchGateway<TContext>
    {
        /// <summary>
        ///  流体的分支网关基类
        ///    所有分支都失败会触发block
        /// </summary>
        protected BaseBranchGateway(string pipeCode = null) : base(pipeCode, PipeType.BranchGateway)
        {
        }

        /// <summary>
        ///  所有分支管道
        /// </summary>
        protected IReadOnlyList<IPipeMeta> BranchPipes => _branchItems.Select(bw => bw.Pipe).ToList();

        /// <summary>
        ///  条件分支过滤处理
        /// </summary>
        /// <param name="branchContext">当前传入分支网关的上下文</param>
        /// <param name="branch">等待过滤的分支</param>
        /// <returns> True-执行当前分支， False-不执行当前分支 </returns>
        protected virtual bool FilterBranchCondition(TContext branchContext, IPipeMeta branch)
        {
            return true;
        }


        #region 条件追加处理

        internal Dictionary<IPipeMeta, Func<TContext, bool>> interConditions;

        void IBranchGateway<TContext>.SetCondition(IPipeMeta pipe, Func<TContext, bool> condition)
        {
            if (condition == null )
                throw new ArgumentNullException(nameof(condition), $"指向 {pipe.PipeCode} 的条件判断不能为空!");

            interConditions ??= new Dictionary<IPipeMeta, Func<TContext, bool>>();

            if (interConditions .ContainsKey(pipe))
                throw new DuplicateNameException(string.Concat(PipeCode, "分支网关 存在不同条件但相同指向的分支！"));
            
            interConditions[pipe] =   condition;
        }
        
        #endregion


        #region 管道内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficSignal<TContext, TContext>> InterProcessing(TContext context)
        {
            var nextPipes = FilterUseableBranches(context);
            if (nextPipes == null || !nextPipes.Any())
            {
                return new TrafficSignal<TContext, TContext>(SignalFlag.Yellow_Wait, context,
                    context, "未能找到可执行的后续节点!");
            }

            var parallelPipes = nextPipes.Select(p => p.InterPreCall(context));

            var res = (await Task.WhenAll(parallelPipes)).All(r => r.signal == SignalFlag.Green_Pass)
                ? new TrafficSignal<TContext, TContext>(SignalFlag.Green_Pass, context, context)
                : new TrafficSignal<TContext, TContext>(SignalFlag.Yellow_Wait, context, context, "分支子节点并未全部成功！");

            return res;
        }

        private IList<IBranchWrap> FilterUseableBranches(TContext context)
        {
            if (_branchItems == null)
                return null;

            var nextPipes =new List<IBranchWrap>();

            foreach (var branchItem in _branchItems)
            {
                if (!FilterBranchCondition(context, branchItem.Pipe))
                    continue;
                
                if (interConditions!=null 
                    && interConditions.ContainsKey(branchItem.Pipe) 
                    && !interConditions[branchItem.Pipe].Invoke(context))
                    continue;
                
                nextPipes.Add(branchItem);
            }
            return nextPipes;
        }

        #endregion


        #region 管道连接

        // 分支网关的下级处理由自己控制
        internal override Task<TrafficSignal> ToNextThrough(TContext nextInContext)
        {
            return InterUtil.GreenTrafficSignalTask;
        }

        internal override void InterAppend(IPipeInPart<TContext> nextPipe)
        {
            Add(nextPipe);
        }

        internal override void InterAppend(IPipeInPart<Empty> nextPipe)
        {
            Add(nextPipe);
        }

        private List<IBranchWrap> _branchItems;

        internal void Add(IPipeInPart<TContext> pipe)
        {
            if (pipe == null)
            {
                throw new ArgumentNullException(nameof(pipe), " 不能为空！");
            }

            _branchItems ??= new List<IBranchWrap>();
            _branchItems.Add(new BranchNodeWrap<TContext>(pipe));
        }


        internal void Add(IPipeInPart<Empty> pipe)
        {
            if (pipe == null)
            {
                throw new ArgumentNullException(nameof(pipe), " 不能为空！");
            }

            _branchItems ??= new List<IBranchWrap>();
            _branchItems.Add(new BranchNodeWrap(pipe));
        }

        #endregion

        #region 内部初始化

        internal override void InterInitialContainer(IPipeLine flowContainer)
        {
            LineContainer = flowContainer;
            WatchProxy    = flowContainer.GetWatchProxy();

            if (_branchItems == null || !_branchItems.Any())
            {
                throw new ArgumentNullException($"分支网关({PipeCode})并没有可用下游管道");
            }

            _branchItems.ForEach(b => b.InterInitialContainer(flowContainer));
        }

        #endregion

        #region 内部路由处理

        internal override void InterFormatLink(string prePipeCode, bool isSelf = false)
        {
            base.InterFormatLink(prePipeCode, isSelf);
            _branchItems.ForEach(b => b.InterFormatLink(prePipeCode, isSelf));
        }
        
        #endregion
    }
}
