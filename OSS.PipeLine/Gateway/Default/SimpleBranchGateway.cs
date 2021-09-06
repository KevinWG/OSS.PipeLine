#region Copyright (C) 2021 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.PipeLine -  简单分支网关实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2021-7-5
*       
*****************************************************************************/

#endregion

using System;
using OSS.Pipeline;
using OSS.Pipeline.Interface;

namespace OSS.PipeLine.Gateway.Default
{
    public class SimpleBranchGateway<TContext>:BaseBranchGateway<TContext>
    {
        private readonly Func<TContext, IPipeMeta, string, bool> _conditionFilter;

        public SimpleBranchGateway(string pipeCode=null, Func<TContext, IPipeMeta, string, bool> branchConditionfilter = null) :
            base(pipeCode)
        {
            _conditionFilter = branchConditionfilter;
        }

        protected override bool FilterBranchCondition(TContext branchContext, IPipeMeta branch, string prePipeCode)
        {
            return _conditionFilter?.Invoke(branchContext, branch, prePipeCode)??true;
        }
    }
}
