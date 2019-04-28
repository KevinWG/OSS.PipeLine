using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.TaskFlow.Node.Mos;
using OSS.TaskFlow.Tasks;
using OSS.TaskFlow.Tasks.MetaMos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseNode
    {
        public InstanceType InstanceType { get; protected set; }

        protected BaseNode()
        {
            InstanceType = InstanceType.Stand;
        }
        
        protected abstract Task<Dictionary<TaskMeta,BaseTask>> GetTaskMetas(NodeContext context);
    }

}
