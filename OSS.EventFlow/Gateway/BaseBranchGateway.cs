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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSS.EventFlow.Connector;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Gateway
{
    /// <summary>
    /// 流体的分支网关基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseBranchGateway<TContext> : BasePipe<TContext>
        where TContext : IPipeContext
    {
        /// <summary>
        ///  流体的分支网关基类
        /// </summary>
        protected BaseBranchGateway() : base(PipeType.Gateway)
        {
        }

        internal override async Task<bool> Handling(TContext context)
        {
            var nextPipes = FilterNextPipes(_branchItems, context);
            if (nextPipes == null || !nextPipes.Any())
            {
                return false;
            }

            var parallelPipes = nextPipes.Select(p => p.Start(context));
            await Task.WhenAll(parallelPipes);
            return true;
        }

        /// <summary>
        ///   过滤可分发下路的分支
        ///   filer available pipes that can go to next during the runtime;
        /// </summary>
        /// <param name="branchItems"></param>
        /// <param name="context"></param>
        /// <returns>如果为空，则触发block</returns>
        protected abstract IEnumerable<BasePipe<TContext>> FilterNextPipes(List<BasePipe<TContext>> branchItems,
            TContext context);

        private List<BasePipe<TContext>> _branchItems;

        /// <summary>
        ///   添加分支       
        /// </summary>
        /// <param name="branchPipe"></param>
        public BaseSinglePipe<TContext, NextOutContext> AddBranchPipe<NextOutContext>(
            BaseSinglePipe<TContext, NextOutContext> branchPipe)
            where NextOutContext : IPipeContext
        {
            if (branchPipe == null)
            {
                throw new ArgumentNullException(nameof(branchPipe), " 不能为空！");
            }

            _branchItems ??= new List<BasePipe<TContext>>();
            _branchItems.Add(branchPipe);

            return branchPipe;
        }


        internal override string InterToRoute(string endPipeCode)
        {
            var jsonBuilder = new StringBuilder();

            jsonBuilder.Append("{ \"pipe_code\":\"").Append(pipe_code).Append("\"")
            .Append(",\"pipe_type\":").Append((int) pipe_type)
            .Append(",\"nexts\":[");
            if (_branchItems.Any())
            {
                jsonBuilder.Append(string.Join(",", _branchItems.Select(bp => bp.InterToRoute(endPipeCode))));
            }
            jsonBuilder.Append("]}");

            return jsonBuilder.ToString();
        }
    }

    /// <summary>
    ///  网关扩展类
    /// </summary>
    public static class BaseBranchGatewayExtension
    {
        /// <summary>
        ///  添加转换分支管道
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <param name="gateway"></param>
        /// <param name="convertFunc"></param>
        /// <returns></returns>
        public static BaseSinglePipe<TContext, NextOutContext> AddConvertBranchPipe<TContext, NextOutContext>(
            this BaseBranchGateway<TContext> gateway, Func<TContext, NextOutContext> convertFunc)
            where NextOutContext : IPipeContext
            where TContext : IPipeContext
        {
            var nextConverter = new DefaultConnector<TContext, NextOutContext>(convertFunc);
            return gateway.AddBranchPipe(nextConverter);
        }
    }
}
