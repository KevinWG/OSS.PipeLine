using System.Threading.Tasks;
using OSS.Common.ComModels;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  基础工作者
    /// </summary>
    public abstract partial class BaseNode<TPara> : BaseNode // : INode<TPara>,IFlowNode
    {
        public abstract Task<ResultMo> Call(TPara para);

        internal override Task<ResultMo> Call(ExcuteReq fReq, object taskData)
        {
            return Call((TPara) taskData);
        }
    }

    public abstract class BaseNode
    {
        internal abstract Task<ResultMo> Call(ExcuteReq fReq, object taskData);
    }

}
