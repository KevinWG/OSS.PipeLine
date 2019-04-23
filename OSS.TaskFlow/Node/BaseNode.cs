using System.Threading.Tasks;
using OSS.EventFlow.Tasks.Mos;
using OSS.TaskFlow.FlowLine.Mos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  基础工作者
    /// </summary>
    public abstract partial class BaseNode<TPara>:BaseNode// : INode<TPara>,IFlowNode
    {
        public abstract Task<TaskResultMo> Call(TPara para);

        internal override Task<TaskResultMo> Call(FlowReq fReq, object taskData)
        {
            return Call((TPara)taskData);
        }
    }

    public abstract class BaseNode
    {
        internal abstract Task<TaskResultMo> Call(FlowReq fReq, object taskData);
    }

}
