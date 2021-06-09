using OSS.Pipeline.Base;

namespace OSS.Pipeline.Interface
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TOutContext"></typeparam>
    public interface IPipelineAppender<TInContext,TOutContext>
    {
       internal BaseInPipePart<TInContext>    StartPipe   { get; set; }
       internal IOutPipeAppender<TOutContext> EndAppender { get; set; }
    }

    public interface IPipelineBranchAppender<TInContext, TOutContext>
    {
        internal BaseInPipePart<TInContext>     StartPipe   { get; set; }
        internal BaseBranchGateway<TOutContext> EndAppender { get; set; }
    }


}
