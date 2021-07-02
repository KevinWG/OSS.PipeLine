using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline
{

    /// <summary>
    ///  简单pipeline，所有管道上下文相同
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class SimplePipeline<TContext> : Pipeline<TContext, TContext>, ISimplePipeline<TContext>
    {
        public SimplePipeline(string pipeCode, BaseInPipePart<TContext> startPipe, IPipeAppender<TContext> endPipeAppender) : base(pipeCode, startPipe, endPipeAppender)
        {
        }

        public SimplePipeline(string pipeCode, BaseInPipePart<TContext> startPipe, IPipeAppender<TContext> endPipeAppender, PipeLineOption option) : base(pipeCode, startPipe, endPipeAppender, option)
        {
        }
    }
}
