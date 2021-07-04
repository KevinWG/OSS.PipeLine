using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.SimplePipeline
{
    internal class SimplePipeline<TContext>:Pipeline<TContext,TContext>, ISimplePipeline<TContext>
    { 
        /// <inheritdoc />
        public SimplePipeline(string pipeCode, BaseInPipePart<TContext> startPipe, IPipeAppender<TContext> endPipeAppender, PipeLineOption option) : base(pipeCode, startPipe, endPipeAppender, option)
        {
        }
    }
}
