﻿#region Copyright (C) 2021 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.PipeLine -  简单分支网关实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2021-7-5
*       
*****************************************************************************/

#endregion

using OSS.Pipeline.Interface;
using System;

namespace OSS.Pipeline
{
    /// <summary>
    ///  简单分支
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class SimpleBranchGateway<TContext> : BaseBranchGateway<TContext>
    {
        private readonly Func<TContext, IPipeMeta,  bool> _conditionFilter;

        /// <summary>
        /// 简单分支
        /// </summary>
        /// <param name="pipeCode"></param>
        /// <param name="branchConditionfilter"></param>
        public SimpleBranchGateway(Func<TContext, IPipeMeta,  bool> branchConditionfilter = null,string pipeCode = null) :
            base(pipeCode)
        {
            _conditionFilter = branchConditionfilter;
        }
        
        /// <inheritdoc />
        protected override bool FilterBranchCondition(TContext branchContext, IPipeMeta branch)
        {
            return _conditionFilter?.Invoke(branchContext, branch) ?? true;
        }
    }
}
