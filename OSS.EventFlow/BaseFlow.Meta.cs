using OSS.EventFlow.MetaMos;
using OSS.EventTask.MetaMos;

namespace OSS.EventFlow
{
    /// <summary>
    /// 流运行时元数据信息
    /// </summary>
    public abstract partial class BaseFlow:BaseMeta<FlowMeta>
    {
        public BaseFlow()
        {

        }

        public BaseFlow(FlowMeta meta) : base(meta)
        {

        }

    }
}
