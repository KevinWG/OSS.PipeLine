using System.Threading.Tasks;
using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline
{
    /// <inheritdoc />
    public class EmptyEntryPipeline<TContext> : Pipeline<Empty, TContext>
    {
        /// <inheritdoc />
        public EmptyEntryPipeline(string pipeCode,BaseInPipePart<Empty> startPipe, IPipeAppender<TContext> endPipeAppender) 
            : base(pipeCode,startPipe, endPipeAppender)
        {
        }

        /// <inheritdoc />
        public EmptyEntryPipeline(string pipeCode, BaseInPipePart<Empty> startPipe, IPipeAppender<TContext> endPipeAppender, PipeLineOption option)
            : base(pipeCode, startPipe, endPipeAppender, option)
        {

        }

        #region 管道启动
        /// <summary>
        ///  启动
        /// </summary>
        /// <returns></returns>
        public Task Execute()
        {
            return InterPreCall(Empty.Default);
        }

        #endregion
    }
}