using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;
using OSS.Pipeline.InterImpls.Pipeline;

namespace OSS.Pipeline
{
    public static class PipelineFactory
    {
        /// <summary>
        ///  添加第一个节点
        /// </summary>
        /// <typeparam name="TPara"></typeparam>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="startPipe"></param>
        /// <returns></returns>
        public static IPipelineAppender<TIn, TOut> Start<TIn, TPara, TOut>( BasePipe<TIn, TPara, TOut> startPipe)
        {
            return Set(new InterPipelineAppender<TIn, TOut>(), startPipe, startPipe);
        }

        /// <summary>
        ///  添加第一个节点
        /// </summary>
        /// <typeparam name="TPara"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="startPipe"></param>
        /// <returns></returns>
        public static IPipelineAppender<EmptyContext, TOut> Start< TPara, TOut>(BasePipe<EmptyContext, TPara, TOut> startPipe)
        {
            return Set(new InterPipelineAppender<EmptyContext, TOut>(), startPipe, startPipe);
        }


        /// <summary>
        ///  添加第一个节点
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="startPipe"></param>
        /// <returns></returns>
        public static IPipelineBranchAppender<TIn, TIn> Start<TIn>(BaseBranchGateway<TIn> startPipe)
        {
            return Set(new InterPipelineBranchAppender<TIn, TIn>(), startPipe, startPipe);
        }




        internal static IPipelineAppender<TIn, TOut> Set<TIn, TOut>(IPipelineAppender<TIn, TOut> appender,
            BaseInPipePart<TIn> startPipe,
            IPipeAppender<TOut> endAppender)
        {
            appender.StartPipe   = startPipe;
            appender.EndAppender = endAppender;
            return appender;
        }

        internal static IPipelineBranchAppender<TIn, TOut> Set<TIn, TOut>(IPipelineBranchAppender<TIn, TOut> appender,
            BaseInPipePart<TIn> startPipe,
            BaseBranchGateway<TOut> endAppender)
        {
            appender.StartPipe   = startPipe;
            appender.EndAppender = endAppender;
            return appender;
        }
    }
}
