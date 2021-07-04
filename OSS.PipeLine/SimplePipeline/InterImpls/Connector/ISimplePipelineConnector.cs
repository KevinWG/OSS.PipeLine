using OSS.Pipeline.Pipeline.InterImpls.Connector;

namespace OSS.Pipeline.SimplePipeline.InterImpls.Connector
{
    /// <inheritdoc />
    public interface ISimplePipelineConnector<TContext> : IPipelineConnector<TContext, TContext>
    {
    }
}
