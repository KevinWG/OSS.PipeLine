using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.TaskFlow.Tasks.MetaMos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  基础工作者
    /// </summary>
    public abstract partial class BaseNode<TReq> : BaseNode // : INode<TReq>,IFlowNode
    {
        internal virtual async Task<ResultMo> Call(TaskContext<TReq> context)
        {
            var taskMetas =await GetTaskMetas(context);
            var task = taskMetas.FirstOrDefault(t => t.task_key == context.task_key);
            if (task==null)
            {
                return new ResultMo(ResultTypes.UnKnowOperate, "未知的处理操作！");
            }

            return new ResultMo();
        }


        public abstract Task<IList<TaskMeta>> GetTaskMetas(TaskBaseContext context);

        internal override Task<ResultMo> Call(ExcuteReq fReq, object flowData)
        {
            var context=new TaskContext<TReq>();

            // 初始化相关上下文信息
            return Call(context);
        }
    }

    public abstract class BaseNode
    {
        internal abstract Task<ResultMo> Call(ExcuteReq fReq, object flowData);
    }

}
