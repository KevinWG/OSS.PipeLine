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
using OSS.Pipeline.InterImpls.GateWay;

namespace OSS.Pipeline
{
    /// <summary>
    /// 流体的分支网关基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseBranchGateway<TContext> : BaseThreeWayPipe<TContext,TContext, TContext>
    {
        /// <summary>
        ///  流体的分支网关基类
        ///    所有分支都失败会触发block
        /// </summary>
        protected BaseBranchGateway() : base(PipeType.BranchGateway)
        {
        }

        /// <inheritdoc />
        internal override async Task<TrafficResult<TContext, TContext>> InterProcessPackage(TContext context, string prePipeCode)
        {
            var nextPipes = SelectNextPipes( context, _branchItems);
            if (nextPipes == null || !nextPipes.Any())
                return new TrafficResult<TContext, TContext>(SignalFlag.Red_Block, PipeCode, "未能找到可执行的后续节点!", context,context);

            var parallelPipes = nextPipes.Select(p => p.InterPreCall(context,PipeCode));

            var res = (await Task.WhenAll(parallelPipes)).Any(r => r.signal == SignalFlag.Green_Pass)
                ? new TrafficResult<TContext, TContext>(SignalFlag.Green_Pass, string.Empty, string.Empty, context, context)
                : new TrafficResult<TContext, TContext>(SignalFlag.Red_Block, PipeCode, "所有分支运行失败！", context, context);

            return res;
        }

        /// <summary>
        ///   过滤可分发下路的分支
        ///   filer available pipes that can go to next during the runtime;
        /// </summary>
        /// <param name="branchNodePool"></param>
        /// <param name="context"></param>
        /// <returns>如果为空，则触发block</returns>
        protected virtual IEnumerable<IBranchNodePipe> SelectNextPipes(TContext context,
            List<IBranchNodePipe> branchNodePool)
        {
            return branchNodePool;
        }

        #region 管道连接
        
        internal override void InterAppend(BaseInPipePart<TContext> nextPipe)
        {
            Add(nextPipe);
            nextPipe.InterAppendTo(this);
        }

        internal override void InterAppend(BaseInPipePart<Empty> nextPipe)
        {
            Add(nextPipe);
            nextPipe.InterAppendTo(this);
        }


        private List<IBranchNodePipe> _branchItems;

        private void Add(BaseInPipePart<TContext> pipe)
        {
            if (pipe == null)
            {
                throw new ArgumentNullException(nameof(pipe), " 不能为空！");
            }

            _branchItems ??= new List<IBranchNodePipe>();

            _branchItems.Add(new BranchNodeWrap<TContext>(pipe));
        }


        private void Add(BaseInPipePart<Empty> pipe)
        {
            if (pipe == null)
            {
                throw new ArgumentNullException(nameof(pipe), " 不能为空！");
            }

            _branchItems ??= new List<IBranchNodePipe>();
            _branchItems.Add(new BranchNodeWrap(pipe));
        }

        #endregion

        #region 内部初始化

        internal override void InterInitialContainer(IPipeLine flowContainer)
        {
            LineContainer = flowContainer;
            WatchProxy    = flowContainer.GetProxy();

            if (_branchItems == null || !_branchItems.Any())
            {
                throw new ArgumentNullException($"分支网关({PipeCode})并没有可用下游管道");
            }

            _branchItems.ForEach(b => b.InterInitialContainer(flowContainer));
        }

        #endregion

        #region 内部路由处理
        
        internal override PipeRoute InterToRoute(bool isFlowSelf = false)
        {
            var pipe = new PipeRoute()
            {
                pipe_code = PipeCode,
                pipe_type = PipeType
            };

            if (Equals(LineContainer.EndPipe))
            {
                return pipe;
            }

            if (_branchItems.Any())
            {
                pipe.nexts = _branchItems.Select(bp => bp.InterToRoute(isFlowSelf)).ToList();
            }
            return pipe;
        }

        #endregion
    }
}
