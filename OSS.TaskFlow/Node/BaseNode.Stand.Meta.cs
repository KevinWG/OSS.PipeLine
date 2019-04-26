using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Node.MetaMos;
using OSS.TaskFlow.Node.Mos;
using OSS.TaskFlow.Tasks;
using OSS.TaskFlow.Tasks.MetaMos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseNode<TReq>
    {
        /// <summary>
        ///  节点信息
        /// </summary>
        public NodeMeta NodeMeta { get; set; }

        protected abstract Task<ResultListMo<TaskMeta>> GetTaskMetas(NodeContext context);

        protected abstract BaseTask GetTaskByMeta(TaskMeta meta);
    }

}
