namespace OSS.EventFlow.Mos
{
    /// <summary>
    /// 流体上下文
    /// </summary>
    public abstract class FlowContext
    {
       
    }

    /// <inheritdoc />
    public abstract class FlowContext<IdType>: FlowContext
    {
        public IdType id
        {
            get;
            set;
        }
    }

}
