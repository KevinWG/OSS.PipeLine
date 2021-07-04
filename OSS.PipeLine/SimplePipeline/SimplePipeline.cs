using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.SimplePipeline
{
    /// <inheritdoc />
    public class SimplePipeline<TContext>:Pipeline<TContext,TContext>
    {
        /// <inheritdoc />
        public SimplePipeline(string pipeCode, BaseInPipePart<TContext> startPipe, IPipeAppender<TContext> endPipeAppender) : base(pipeCode, startPipe, endPipeAppender)
        {
        }

        /// <inheritdoc />
        public SimplePipeline(string pipeCode, BaseInPipePart<TContext> startPipe, IPipeAppender<TContext> endPipeAppender, PipeLineOption option) : base(pipeCode, startPipe, endPipeAppender, option)
        {
        }
    }
}
