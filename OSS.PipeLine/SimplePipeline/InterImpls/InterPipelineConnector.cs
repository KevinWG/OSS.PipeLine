#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  流体追加器内部实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;
using OSS.Pipeline.Pipeline.InterImpls.Connector;

namespace OSS.Pipeline.SimplePipeline.InterImpls.Connector
{

    internal class InterSimplePipelineConnector<TContext>
        : InterPipelineConnector<TContext, TContext> , ISimplePipelineConnector<TContext>
    {
        public InterSimplePipelineConnector(BaseInPipePart<TContext> startPipe, IPipeAppender<TContext> endPipe) : base(startPipe, endPipe)
        {
        }
    }
}
