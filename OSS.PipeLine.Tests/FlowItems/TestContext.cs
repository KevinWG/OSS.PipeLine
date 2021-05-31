using OSS.EventFlow.Mos;

namespace OSS.TaskFlow.Tests.FlowItems
{
    /// <inheritdoc />
    public abstract class TestContext<IdType> : IPipeContext
    {
        /// <summary>
        ///  id 编号
        /// </summary>
        public IdType id
        {
            get;
            set;
        }
    }

}
