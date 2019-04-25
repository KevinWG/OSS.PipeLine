using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Node.MetaMos;
using OSS.TaskFlow.Tasks.MetaMos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  基础工作者
    /// </summary>
    public abstract partial class BaseNode<TReq>
    {
        /// <summary>
        ///  节点信息
        /// </summary>
        public NodeMeta NodeMeta { get; set; }

        public abstract Task<ResultListMo<TaskMeta>> GetTaskMetas(TaskBaseContext context);
    }
}
