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

using System.Collections.Generic;
using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.InterImpls.Pipeline
{
    internal class InterPipelineAppender<TInContext, TOutContext> : IPipelineAppender<TInContext, TOutContext>
    {

        public InterPipelineAppender(BaseInPipePart<TInContext> startPipe, IPipeAppender<TOutContext> endPipe)
        {
            Initial(this, startPipe, endPipe);
        }
        
        private static void Initial(IPipelineAppender<TInContext, TOutContext> pipelineAppender,
            BaseInPipePart<TInContext> startPipe, IPipeAppender<TOutContext> endPipe)
        {
            pipelineAppender.StartPipe   = startPipe;
            pipelineAppender.EndAppender = endPipe;
        }

        BaseInPipePart<TInContext> IPipelineAppender<TInContext, TOutContext>.StartPipe { get; set; }

        IPipeAppender<TOutContext> IPipelineAppender<TInContext, TOutContext>.EndAppender { get; set; }
    }

    internal class
        InterPipelineBranchAppender<TInContext, TOutContext> : IPipelineBranchAppender<TInContext, TOutContext>
    {
        public InterPipelineBranchAppender(BaseInPipePart<TInContext> startPipe, BaseBranchGateway<TOutContext> endPipe)
        {
            Initial(this, startPipe, endPipe);
        }

        private static void Initial(IPipelineBranchAppender<TInContext, TOutContext> pipelineAppender,
            BaseInPipePart<TInContext> startPipe, BaseBranchGateway<TOutContext> endPipe)
        {
            pipelineAppender.StartPipe     = startPipe;
            pipelineAppender.EndBranchPipe = endPipe;
        }

        BaseInPipePart<TInContext> IPipelineBranchAppender<TInContext, TOutContext>.    StartPipe   { get; set; }
        BaseBranchGateway<TOutContext> IPipelineBranchAppender<TInContext, TOutContext>.EndBranchPipe { get; set; }
    }


    internal class
        InterPipelineMsgEnumerableAppender<TInContext, TMsgEnumerable, TMsg> : IPipelineMsgEnumerableAppender<TInContext, TMsgEnumerable, TMsg>
            where TMsgEnumerable:IEnumerable<TMsg> 
    {
        public InterPipelineMsgEnumerableAppender(BaseInPipePart<TInContext> startPipe, BaseMsgEnumerator<TMsgEnumerable, TMsg> endPipe)
        {
            Initial(this, startPipe, endPipe);
        }

        private static void Initial(IPipelineMsgEnumerableAppender<TInContext, TMsgEnumerable, TMsg> pipelineAppender,
            BaseInPipePart<TInContext> startPipe, BaseMsgEnumerator<TMsgEnumerable, TMsg> endPipe)
        {
            pipelineAppender.StartPipe = startPipe;
            pipelineAppender.EndPipe   = endPipe;
        }

        BaseInPipePart<TInContext> IPipelineMsgEnumerableAppender<TInContext, TMsgEnumerable, TMsg>.    StartPipe     { get; set; }
        BaseMsgEnumerator<TMsgEnumerable, TMsg> IPipelineMsgEnumerableAppender<TInContext, TMsgEnumerable, TMsg>.EndPipe { get; set; }
    }

    //internal class
    //    InterPipelineInterceptAppender<TInContext, TOutContext> : IPipelineInterceptAppender<TInContext, TOutContext>
    //{
    //    public InterPipelineInterceptAppender(BaseInPipePart<TInContext> startPipe, BaseInterceptPipe<TOutContext> endPipe)
    //    {
    //        Initial(this, startPipe, endPipe);
    //    }

    //    private static void Initial(IPipelineInterceptAppender<TInContext, TOutContext> pipelineAppender,
    //        BaseInPipePart<TInContext> startPipe, BaseInterceptPipe<TOutContext> endPipe)
    //    {
    //        pipelineAppender.StartPipe = startPipe;
    //        pipelineAppender.EndPipe   = endPipe;
    //    }
    //    BaseInPipePart<TInContext> IPipelineInterceptAppender<TInContext, TOutContext>.    StartPipe     { get; set; }
    //    BaseInterceptPipe<TOutContext> IPipelineInterceptAppender<TInContext, TOutContext>.EndPipe { get; set; }
    //}
}
