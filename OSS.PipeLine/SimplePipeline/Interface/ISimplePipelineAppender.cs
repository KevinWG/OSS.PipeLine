#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  管道追加器接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using OSS.Pipeline.Base;

namespace OSS.Pipeline.Interface
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface ISimplePipelineAppender<TContext>
    {
       internal BaseInPipePart<TContext> StartPipe   { get; set; }
       internal IPipeAppender<TContext>  EndAppender { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface ISimplePipelineBranchAppender<TContext>
    {
        internal BaseInPipePart<TContext>    StartPipe     { get; set; }
        internal BaseBranchGateway<TContext> EndBranchPipe { get; set; }
    }

    
}
