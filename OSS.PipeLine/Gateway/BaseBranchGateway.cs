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
using System.Linq;
using System.Threading.Tasks;
using OSS.Pipeline.Base;
using OSS.Pipeline.Gateway.InterImpls;
using OSS.Pipeline.InterImpls;

namespace OSS.Pipeline
{
    /// <summary>
    /// 流体的分支网关基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseBranchGateway<TContext> : BaseThreeWayPipe<TContext, TContext, TContext>
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
        protected IReadOnlyList<IPipeMeta> BranchPipes
        {
            get => _branchItems.Select(bw => bw.Pipe).ToList();
        }

        /// <summary>
        ///  条件分支过滤处理
        /// </summary>
        /// <param name="branchContext">当前传入分支网关的上下文</param>
        /// <param name="branch">等待过滤的分支</param>
        /// <param name="prePipeCode">当前网关分支的上游管道编码</param>
        /// <returns> True-执行当前分支， False-不执行当前分支 </returns>
        protected virtual bool FilterBranchCondition(TContext branchContext, IPipeMeta branch)
        {
            return true;
        }


        #region 管道内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficResult<TContext, TContext>> InterProcessPackage(TContext context)
        {
            IList<IBranchWrap> nextPipes;
            if (_branchItems == null || !_branchItems.Any()
                                     || !(nextPipes = _branchItems
                                             .Where(bw => FilterBranchCondition(context, bw.Pipe))
                                             .ToList())
                                         .Any())
            {
                return new TrafficResult<TContext, TContext>(SignalFlag.Red_Block, PipeCode, "未能找到可执行的后续节点!", context,
                    context);
            }

            var parallelPipes = nextPipes.Select(p => p.InterPreCall(context));

            var res = (await Task.WhenAll(parallelPipes)).Any(r => r.signal == SignalFlag.Green_Pass)
                ? new TrafficResult<TContext, TContext>(SignalFlag.Green_Pass, string.Empty, string.Empty, context,
                    context)
                : new TrafficResult<TContext, TContext>(SignalFlag.Red_Block, PipeCode, "所有分支运行失败！", context, context);

            return res;
        }

        #endregion


        #region 管道连接

        // 分支网关的下级处理由自己控制
        internal override Task<TrafficResult> ToNextThrough(TContext nextInContext)
        {
            return InterUtil.GreenTrafficResultTask;
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
