using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.ComUtils;
using OSS.TaskFlow.Tasks;
using OSS.TaskFlow.Tasks.MetaMos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node
{

    public enum NodeType
    {
        Sequence,
        Parallel
    }

    /// <summary>
    ///  基础工作者
    /// </summary>
    public abstract partial class BaseNode<TReq> : BaseNode // : INode<TReq>,IFlowNode
    {


        public async Task<ResultMo> Excute(TaskContext<TReq> context)
        {
            var taskMetas = await GetTaskMetas(context);
            if (!taskMetas.IsSuccess())
                return new ResultMo(ResultTypes.UnKnowOperate, "未知的处理操作！");
            

            return new ResultMo();
        }
        



        internal override Task<ResultMo> Excute(ExcuteReq fReq, object flowData)
        {
            var context = new TaskContext<TReq>();

            // 初始化相关上下文信息
            return Excute(context);
        }

        
        public abstract Task<ResultListMo<TaskMeta>> GetTaskMetas(TaskBaseContext context);
        
        private static readonly IDictionary<string ,BaseTask> _taskContainer=new Dictionary<string, BaseTask>();
        private static ResultMo<BaseTask> GetTask(string taskKey)
        { 
            return _taskContainer.TryGetValue(taskKey, out BaseTask task)
                ? new ResultMo<BaseTask>(task)
                : new ResultMo<BaseTask>(ResultTypes.UnKnowOperate, "未找到对应的任务处理器！");
        }

        private static void RegisteTask<TTask>(string taskKey, bool isSingle = true)
            where TTask : BaseTask,new()
        {
            var task = default(TTask);
            //if (!InsContainer<TTask>.TryGet(out task))
            //{
                InsContainer<TTask>.Set<TTask>(isSingle);
                task = InsContainer<TTask>.Instance;
            //}

            RegisteTask(taskKey, task);
        }
        private static void RegisteTask(string taskKey, BaseTask task)
        {
            if (_taskContainer.ContainsKey(taskKey))
            {
                throw new ArgumentException($"have more than one task name with '{taskKey}' in this node!",nameof(taskKey));
            }
            _taskContainer.Add(taskKey, task); 
        }
    }

    public abstract class BaseNode
    {
        internal abstract Task<ResultMo> Excute(ExcuteReq fReq, object flowData);
    }

}
