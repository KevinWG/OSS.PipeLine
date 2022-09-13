using System.Threading.Tasks;
using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline
{
    /// <inheritdoc />
    public class EmptyEntryPipeline<TContext> : Pipeline<Empty, TContext>
    {
        /// <inheritdoc />
        public EmptyEntryPipeline(IPipeInPart<Empty> startPipe, IPipeAppender<TContext> endPipeAppender, string pipeCode=null) 
            : base(startPipe, endPipeAppender, pipeCode)
        {
        }

        /// <inheritdoc />
        public EmptyEntryPipeline(IPipeInPart<Empty> startPipe, IPipeAppender<TContext> endPipeAppender, PipeLineOption option,string pipeCode= null)
            : base( startPipe, endPipeAppender, option,pipeCode)
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